using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Daxone_Testing.Data;
using Daxone_Testing.Data.Repositories;
using Daxone_Testing.Models;
using Daxone_Testing.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Daxone_Testing.Areas.Management.Controllers.AdminControllers
{
    [Area("Management")]
    [Authorize(Roles = "Owner,Admin")]
    public class ManageUserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly DaxoneTestingContext _context;

        public ManageUserController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,DaxoneTestingContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> IndexAsync(int Page = 1, int PageSize = 20)
        {
            //Lack of access of current admin to a higher roles
            var userrole = await _userManager
                .FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var curentuserroles = await _userManager.GetRolesAsync(userrole);
            var rankrole = _roleManager.Roles
                .Where(r => r.Name == curentuserroles.FirstOrDefault())
                .Sum(r => r.RankOfRole);
            //
            var user = new UsersInIndexViewModel()
            { 
                Users = _userManager.Users.Select(u => new UsersInIndexViewModel()
                {
                    Id = u.Id,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    UserName = u.UserName,
                    UserRoles = new List<string>(),

                }).OrderBy(u => u.Id).ToList().Skip((Page - 1) * PageSize).Take(PageSize),
                PageSize = PageSize,
                CurrentPage = Page,
                TotalRecords = _userManager.Users.Count(),
                ValidRoles = _roleManager.Roles.Where(r => r.RankOfRole >= rankrole).ToList()
            };
            
            //get roles of any user
            foreach (var item in user.Users)
            {
                var appuser = _userManager.Users.FirstOrDefault(u => u.Id == item.Id);
                var userrol = await _userManager.FindByIdAsync(appuser.Id);
                var userRoles = await _userManager.GetRolesAsync(userrol);
                var c=_context.UserRoles.Where(u=>u.UserId==user.Id);
                item.UserRoles.Add(userRoles.FirstOrDefault());
            }
            return View("/Areas/Management/Views/Admin/Users/Index.cshtml", user);
        }

        [HttpGet]
        public async Task<IActionResult> CreateUserAsync()
        {
            var userrol = await _userManager
                .FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var curentuserroles = await _userManager.GetRolesAsync(userrol);
            var rankrole = _roleManager.Roles.Where(r => r.Name == curentuserroles.FirstOrDefault()).Sum(r => r.RankOfRole);

            var roles = new AddOrEditUsersViewModel()
            {
                ValidRoles = _roleManager.Roles.Where(r => r.RankOfRole >= rankrole).ToList()
            };
            return View("/Areas/Management/Views/Admin/Users/CreateUser.cshtml", roles);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(AddOrEditUsersViewModel user, string role)
        {
            if (ModelState.IsValid)
            {
                var appuser = new ApplicationUser()
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    PasswordHash = user.Password,
                    PhoneNumber = user.PhoneNumber,
                    EmailConfirmed = true
                };
                var creatresult = await _userManager.CreateAsync(appuser, user.Password);
                if (!creatresult.Succeeded)
                {
                    return Json(false);
                }
                var currentuser = await _userManager.FindByIdAsync(appuser.Id);

                var roleresult = await _userManager.AddToRoleAsync(currentuser, role);
                if (!roleresult.Succeeded)
                {
                    return Json("roleresulterror");
                }
                return Json(creatresult.Succeeded);
            }
            return Json(false);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var userrol = await _userManager
                .FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var curentadminroles = await _userManager.GetRolesAsync(userrol);

            var rankrole = _roleManager.Roles
                .Where(r => r.Name == curentadminroles.FirstOrDefault())
                .Sum(r => r.RankOfRole); ;

            var userinedit = await _userManager.FindByIdAsync(id);
            var curentuserroles = await _userManager.GetRolesAsync(userinedit);

            var appUser = await _userManager.FindByIdAsync(id);
            var user = new AddOrEditUsersViewModel()
            {
                Id = appUser.Id,
                UserName = appUser.UserName,
                Email = appUser.Email,
                Password = appUser.PasswordHash,
                PhoneNumber = appUser.PhoneNumber,
                UserRoles = curentuserroles.ToList(),
                ValidRoles = _roleManager.Roles.Where(r => r.RankOfRole >= rankrole).ToList(),
            };
            if (user == null) return NotFound();
            return View("/Areas/Management/Views/Admin/Users/EditUser.cshtml", user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(AddOrEditUsersViewModel usr)
        {
            if (string.IsNullOrEmpty(usr.Id)) return NotFound();

            var user = await _userManager.FindByIdAsync(usr.Id);
            if (usr.UserName != null) user.UserName = usr.UserName;
            if (usr.PhoneNumber != null) user.PhoneNumber = usr.PhoneNumber;
            if (usr.Password != null) await _userManager.ChangePasswordAsync(user, user.PasswordHash, usr.Password);
            if (usr.Email != null) user.Email = usr.Email;
            if (user == null) return NotFound();
            if (usr.UserRoles != null) { var selectedrole = await _userManager.AddToRolesAsync(user, usr.UserRoles); }
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return Json(true);

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Json(false);
        }

        [HttpGet]
        public async Task<IActionResult> AddUserToRole(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var roles = _roleManager.Roles.AsNoTracking().Select(r => r.Name).ToList();
            var userRoles = await _userManager.GetRolesAsync(user);
            var validRoles = roles.Where(r => !userRoles.Contains(r))
                .Select(r => new UserRolesViewModel(r)).ToList();
            var model = new AddUserToRoleViewModel(id, validRoles);

            return View("/Areas/Management/Views/Admin/Users/AddUserToRole.cshtml", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUserToRole(AddUserToRoleViewModel model)
        {
            if (model == null) return NotFound();
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();
            var requestRoles = model.UserRoles.Where(r => r.IsSelected)
                .Select(u => u.RoleName)
                .ToList();
            var result = await _userManager.AddToRolesAsync(user, requestRoles);
            if (result.Succeeded) return RedirectToAction("index");
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View("/Areas/Management/Views/Admin/Users/AddUserToRole.cshtml", model);
        }
        [HttpGet]
        public async Task<IActionResult> RemoveUserFromRole(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var userRoles = await _userManager.GetRolesAsync(user);
            var validroles = userRoles.Select(u => new UserRolesViewModel(u)).ToList();
            var model = new AddUserToRoleViewModel(id, validroles);
            return View("/Areas/Management/Views/Admin/Users/RemoveUserFromRole.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserFromRole(AddUserToRoleViewModel model)
        {
            if (model == null) return NotFound();
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();
            var requestRoles = model.UserRoles.Where(r => r.IsSelected)
                .Select(u => u.RoleName)
                .ToList();
            var result = await _userManager.RemoveFromRolesAsync(user, requestRoles);


            if (result.Succeeded) return RedirectToAction("index");

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View("/Areas/Management/Views/Admin/Users/RemoveUserFromRole.cshtml", model);
        }
        [HttpPost]

        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            await _userManager.DeleteAsync(user);

            return Json(true);
        }

    }
}
