using Daxone_Testing.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using Daxone_Testing.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ZarinpalSandbox;
using Daxone_Testing.Data.Repositories;
using Daxone_Testing.Models.ViewModels;
using Microsoft.AspNetCore.Razor.TagHelpers;


namespace Daxone_Testing.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private DaxoneTestingContext _context;

        public HomeController(ILogger<HomeController> logger, DaxoneTestingContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Microsoft.AspNetCore.Mvc.Route("/{pageId?}")]
        public IActionResult Index(int pageId = 1)
        {
            ViewData["Home"] = "Home";
            #region Paging
            int skip = (pageId - 1) * 9;
            int count = _context.Products.Count();
            ViewBag.pageId = pageId;//active position
            ViewBag.pageCount = count / 9;
            ViewBag.nextPage = pageId += 1;
            ViewBag.previousPage = pageId -= 1;
            if (pageId < 1)
            {
                return RedirectToActionPreserveMethod();
            }
            if (ViewBag.pageCount + 1 < pageId)
            {
                return RedirectPreserveMethod($"/?PageId={ViewBag.pageCount + 1}");
            }
            var page = _context.Products.OrderBy(p => p.Id).Include(p => p.Item).Skip(skip).Take(9).ToList();
            #endregion
            ViewBag.owlsugestroducts1 = _context.Products.Include(p => p.Item).OrderBy(p => Guid.NewGuid()).Take(5).ToList();
            ViewBag.owlsugestroducts2 = _context.Products.Include(p => p.Item).OrderBy(p => Guid.NewGuid()).Take(5).ToList();

            return View(page);
        }


        public IActionResult Details(int id)
        {
            ViewBag.imagesCounter = 0;
            var details = _context.Products
                .Include(p => p.Item)
                .SingleOrDefault(p => p.Id == id);
            if (details == null)
            {
                return NotFound();
            }

            var categories = _context.Products.Where(p => p.Id == id)
                .SelectMany(s => s.CategoryToProducts)
                .Select(ca => ca.Category).ToList();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var vm = new DetailViewModel()
            {
                Product = details,
                Categories = categories,
                ProductPics = _context.ProductPics.Where(p => p.ProductId == details.Id).AsNoTracking(),
                UserFavProducts = _context.Products.Where(p => p.Id == p.FavoriteToProducts.Sum(p => p.ProductId) && p.FavoriteToProducts
                    .Any(i => i.UserFavorites.UserId == userId)).ToList()
            };

            return View(vm);
        }

        [Authorize]
        [Microsoft.AspNetCore.Mvc.Route("Home/AddToCart/{itemid}/{qtyButton}")]
        public IActionResult AddToCart(int itemid, string qtyButton)

        {
            var product = _context.Products.Include(p => p.Item).SingleOrDefault(p => p.ItemId == itemid);
            if (product != null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var factor = _context.Factors.FirstOrDefault(f => f.UserId == userId && f.IsFinaly != true);
                if (factor != null)
                {
                    var factorDetail = _context.FactorDetails.FirstOrDefault(d =>
                        d.FactorId == factor.FactorId && d.ProductId == product.Id);
                    if (factorDetail != null)
                    {
                        factorDetail.Count = int.Parse(qtyButton);
                    }
                    else
                    {
                        _context.FactorDetails.Add(new FactorDetail()
                        {
                            FactorId = factor.FactorId,
                            ProductId = product.Id,
                            Price = product.Item.Price,
                            Count = int.Parse(qtyButton)
                        });
                    }
                }
                else
                {
                    factor = new Factor()
                    {
                        IsFinaly = false,
                        CreatDate = DateTime.Now,
                        UserId = userId,
                    };
                    _context.Factors.Add(factor);
                    _context.SaveChanges();
                    _context.FactorDetails.Add(new FactorDetail()//چون فاکتور وجود ندار ریز فاکتوری نیس پس باید ساخت
                    {
                        FactorId = factor.FactorId,
                        ProductId = product.Id,
                        Price = product.Item.Price,
                        Count = 1
                    });
                }

                _context.SaveChanges();
            }

            return RedirectToAction("ShowCart");
        }

        [Authorize]

        public IActionResult AddToFavorites(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            _context.FavoriteToProducts.Add(new FavoriteToProducts()
            {
                ProductId = productId,
                UserFavorites ={UserId = userId } 
            });
            _context.SaveChanges();

            return Redirect("/Account/MyAccount");
        }
        [Authorize]

        public IActionResult RemoveFromFavorites(int Id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
           var favs= _context.FavoriteToProducts.Where(u=>u.ProductId==Id);
           _context.FavoriteToProducts.RemoveRange(favs);
            _context.SaveChanges();

            return Redirect("/Account/MyAccount");
        }



        [Authorize]
        public IActionResult ShowCart()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            var factor = _context.Factors
                .Where(f => f.UserId == userId && f.IsFinaly == false)
                .Include(f => f.FactorDetails)
                .ThenInclude(c => c.Product).FirstOrDefault();
            return View(factor);
        }


        [Microsoft.AspNetCore.Mvc.Route("ContactUs")]
        public IActionResult ContactUs()
        {
            return View();
        }

        [Authorize]
        public IActionResult RemoveItem(int detailId)
        {
            var factorDetail = _context.FactorDetails.Find(detailId);
            _context.Remove(factorDetail);
            _context.SaveChanges();
            return RedirectToAction("ShowCart");
        }
        [Authorize]
        [Microsoft.AspNetCore.Mvc.Route("Home/RemoveItemInMiniOrder/{detailId}")]
        public IActionResult RemoveItemInMiniOrder(int detailId)
        {
            var factorDetail = _context.FactorDetails.Find(detailId);
            _context.Remove(factorDetail);
            _context.SaveChanges();
            return Json(true);
        }


        public IActionResult Privacy()
        {
            var product = _context.Products.Include(p => p.Item).ToList();
            return View(product);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Payment(int count)
        {
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var Factor = _context.Factors
                .Include(f => f.FactorDetails)
                .FirstOrDefault(f => f.UserId == userId && f.IsFinaly == false);
            if (Factor == null) return NotFound();

            var Fdetail=Factor.FactorDetails.Select(p=>p.ProductId);

            foreach (var item in Fdetail)
            {
                var minusQt= _context.Products.Where(p => p.Id == item).Select(p=>p.Item.QuantityInStuck).ToList();

                foreach (var newQt in minusQt)
                {
                   var qt= _context.Products.Find(item);
                   _context.Items.First(p=>p.Id==qt.ItemId).QuantityInStuck=newQt- 1;
                   _context.SaveChanges();
                }
            }
            

            var payment = new Payment((int)Factor.FactorDetails.Sum(d => d.Price));
            var res = payment.PaymentRequest($"پرداخت فاکتور شماره {Factor.FactorId}",
                "http://localhost:55809/Home/OnlinePayment/" + Factor.FactorId, "hjiloomohamad@gmail.com",
                "09021574683");
            if (res.Result.Status == 100)
            {
                return Json($"https://sandbox.zarinpal.com/pg/StartPay/ {res.Result.Authority}"  );
            }
            else
            {
                return BadRequest();
            }
        }

        
        public IActionResult OnlinePayment(int id)
        {
            if (HttpContext.Request.Query["Status"] != "" &&
                HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" &&
                HttpContext.Request.Query["Authority"] != "")
            {
                string authority = HttpContext.Request.Query["Authority"].ToString();
                var order = _context.Factors.Include(o => o.FactorDetails)
                    .FirstOrDefault(o => o.FactorId == id);
                var payment = new Payment((int)order.FactorDetails.Sum(d => d.Price));
                var res = payment.Verification(authority).Result;
                if (res.Status == 100)
                {
                    
                    order.IsFinaly = true;
                    _context.Factors.Update(order);
                    _context.SaveChanges();
                    ViewBag.code = res.RefId;
                    return View();
                }
            }

            return NotFound();

        }

    }

}

