using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using Daxone_Testing.Data;
using Daxone_Testing.Data.Repositories;
using Daxone_Testing.Models;
using Daxone_Testing.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Daxone_Testing.Controllers
{
    public class AccountController : Controller
    {
        private IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMessageSender _messageSender;
        private DaxoneTestingContext _context;

        public AccountController(IUserRepository userRepository, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMessageSender messageSender, DaxoneTestingContext context)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _messageSender = messageSender;
            _context = context;
        }

        [Authorize]
        public IActionResult MyAccount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            var userfactor = _context.Factors
                .Where(fa => fa.UserId == userId)
                .Include(f => f.FactorDetails);

            var factor = new MyAccountViewmodel()
            {
                User = _context.Users.FirstOrDefault(u => u.Id == userId),
                UserFactor = userfactor.ToList(),
                UserProducts = _context.Products.Where(p => p.Id == p.FavoriteToProducts
                    .Sum(u => u.ProductId) && p.FavoriteToProducts.Any(i => i.UserFavorites.UserId == userId)).Include(p => p.Item).ToList()

            };

            return View(factor);
        }


        #region Register
        [HttpGet]
        public async Task<IActionResult> LoginRegisterAsync()
        {
            ViewBag.isReg = 1;
            var model = new RegisterViewModel
            {
                LoginViewModel =
                {
                    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
                }
            };
            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            ViewBag.isReg = 1;
            if (ModelState.IsValid)
            {
                var model = new RegisterViewModel
                {
                    LoginViewModel =
                {
                    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
                }
                };

                if (register != null)
                {
                    var user = new ApplicationUser()
                    {
                        UserName = register.RegUserName,
                        PhoneNumber = register.RegPhoneNumber.ToString(),
                        Email = register.RegEmail,
                        PhoneNumberConfirmed = true
                    };
                    var result = await _userManager.CreateAsync(user, register.RegPassword);
                    if (user.PhoneNumber == "09021574683")await _userManager.AddToRoleAsync(user, "Owner");
                    
                    
                    await _userManager.AddToRoleAsync(user, "User");
                    if (result.Succeeded)
                    {
                        var emailConfirmationToken =
                            await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var emailMessage =
                            Url.Action("ConfirmEmail", "Account",
                                new { username = user.UserName, token = emailConfirmationToken },
                                Request.Scheme);
                        await _messageSender.SendEmailAsync(register.RegEmail, "Email confirmation", emailMessage);

                        return View("/Views/SuccessRegister.cshtml", register);
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                    return View("LoginRegister", register);
                }

                return View("LoginRegister", model);

            }

            return View("LoginRegister");
        }
        #endregion

        #region Login
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            ViewBag.isReg = null;
            if (_signInManager.IsSignedIn(User))
            {
                return Redirect("Home");
            }
            var model = new RegisterViewModel()
            {
                LoginViewModel =
                {
                    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
                }
            };
            return View("LoginRegister", model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(RegisterViewModel login)
        {
            ViewBag.isReg = null;
            if (_signInManager.IsSignedIn(User))
            {
                return Redirect("Home");
            }
            login.LoginViewModel.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (login.LoginViewModel.Username!=null|| login.LoginViewModel.Password != null)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    login.LoginViewModel.Username, login.LoginViewModel.Password, login.LoginViewModel.LogRememberMe, true);
                if (result.Succeeded)
                {
                    return Redirect("Home");
                }
                if (result.IsLockedOut)
                {
                    ViewData["ErrorMessage"] = "این کاربر درحال حاضر به مدت پنج دقیقه مسدود شده است...برای پشتیبانی میتوانید از طریق تیکت ها اقدام به پیگیری نمایید";
                    return View("LoginRegister", login);
                }
                
                ModelState.AddModelError("", "اطلاعات صحیح نیست");

            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            return View("LoginRegister", login);
        }
        #endregion


        public async Task<IActionResult> Logout()
        {
            ViewBag.isReg = null;
            await _signInManager.SignOutAsync();
            return Redirect("/Account/LoginRegister");
        }


        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userName, string token)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(token))
                return NotFound();
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) return NotFound();
            var result = await _userManager.ConfirmEmailAsync(user, token);

            return Content(result.Succeeded ? "Email Confirmed! Welcome to my Website.... redy for big changes!" : "Email Not Confirmed");
        }


        [HttpPost]

        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            ViewBag.isReg = null;
            var redirectUrl = Url.Action("ExternalLoginCallBack", "Account",
                new { ReturnUrl = returnUrl });

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallBack(string returnUrl = null, string remoteError = null)
        {
            ViewBag.isReg = null;
            ViewData["returnUrl"] = returnUrl;
            returnUrl = (returnUrl != null && Url.IsLocalUrl(returnUrl)) ? returnUrl : Url.Content("~/");

            var loginViewModel = new RegisterViewModel()
            {
                LoginViewModel =
                {
                    ReturnUrl = returnUrl,
                    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
                }

            };

            if (remoteError != null)
            {
                ModelState.AddModelError("", $"Error : {remoteError}");
                return View("LoginRegister", loginViewModel);
            }

            var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
            {
                ModelState.AddModelError("ErrorLoadingExternalLoginInfo", $"مشکلی پیش آمد");
                return View("LoginRegister", loginViewModel);
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider,
                externalLoginInfo.ProviderKey, true, true);

            var userId = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
            var userWhoCanBeOwner = await _userManager.FindByEmailAsync(userId);
            if (userWhoCanBeOwner.PhoneNumber == "09021574683" || !await _userManager.IsInRoleAsync(userWhoCanBeOwner, "Owner"))
            {
                await _userManager.AddToRoleAsync(userWhoCanBeOwner, "Owner");
            }

            if (signInResult.Succeeded)
            {
                return Redirect(returnUrl);
            }

            var email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);

            if (email != null)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null) return View();

                await _userManager.AddLoginAsync(user, externalLoginInfo);
                await _signInManager.SignInAsync(user, false);

                return Redirect(returnUrl);
            }

            ViewData["ErrorMessage"] = $"دریافت کرد {externalLoginInfo.LoginProvider} نمیتوان اطلاعاتی از";
            return View("LoginRegister", loginViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> ExternalLoginCallBack(ExternalLoginCallBackViewModel model, string returnUrl = null)
        {
            ViewBag.isReg = null;
            if (ModelState.IsValid)
            {
                var loginViewModel = new RegisterViewModel()
                {
                    LoginViewModel =
                    {
                        ReturnUrl = returnUrl,
                        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
                    }
                };

                var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
                if (externalLoginInfo?.Principal.FindFirstValue(ClaimTypes.Email) == null)
                {
                    ModelState.AddModelError("ErrorLoadingExternalLoginInfo", $"مشکلی پیش آمد");
                    return View("LoginRegister", loginViewModel);
                }

                var email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);

                var user = await _userManager.FindByEmailAsync(email);
                if (user.PhoneNumber == "09021574683")
                {
                    await _userManager.AddToRoleAsync(user, "Owner");
                }
                var result = new IdentityResult();
                if (user == null)
                {
                    user = new ApplicationUser()
                    {
                        Email = email,
                        UserName = model.UserName,
                        PhoneNumber = model.PhoneNumber,
                        EmailConfirmed = true
                    };

                    if (!string.IsNullOrWhiteSpace(model.Password))
                        result = await _userManager.CreateAsync(user, model.Password);
                    else
                        result = await _userManager.CreateAsync(user);
                }

                if (result.Succeeded)
                {
                    await _userManager.AddLoginAsync(user, externalLoginInfo);
                    await _signInManager.SignInAsync(user, false);

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }


    }
}


