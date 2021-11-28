using Daxone_Testing.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Daxone_Testing.Data;
using Daxone_Testing.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


namespace Daxone_Testing.Controllers
{
    public class ProductController : Controller
    {

        private DaxoneTestingContext _context;

        public ProductController(DaxoneTestingContext context)
        {
            _context = context;
        }
        [Route("/Shop/{groupId?}/{searchKey?}/{pageId?}/")]
        public IActionResult Shop(int groupId, string searchKey, int pageId = 1)
        {
            int skip = ((pageId - 1) * 36);
            var product = _context.CategoryToProducts
                .Where(c => c.CategoryId == groupId)
                .Include(c => c.Product)
                .ThenInclude(p => p.Item)
                .Select(c => c.Product).ToList();
            ViewBag.pageId = pageId;
            ViewBag.pageCount = product.Count() / 36;
            ViewBag.nextPage = pageId += 1;
            ViewBag.previousPage = pageId -= 1;
            ViewData["GroupId"] = groupId;
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var products = new ShopViewModel()
            {
                Products = product.OrderBy(p => p.Id).Skip(skip).Take(36).ToList(),
                UserFavProducts = _context.Products.Where(p=>p.Id==p.FavoriteToProducts.Sum(p=>p.ProductId)&& p.FavoriteToProducts
                    .Any(i => i.UserFavorites.UserId == userId)).ToList()
            };


            if (!string.IsNullOrWhiteSpace(searchKey) && groupId != 0)
            {
                ViewData["SearchKey"] = searchKey;
                var result = product.Where(p => p.Name.Contains(searchKey) || p.Describtion.Contains(searchKey))
                    .ToList();
                products.Products = result.OrderBy(p => p.Id).Skip(skip).Take(36).ToList();
                ViewBag.pageCount = result.Count() / 36;
                if (ViewBag.pageCount + 1 < pageId)
                {
                    return Redirect($"/Shop/{groupId}/{searchKey}/?PageId={ViewBag.pageCount + 1}");
                }
                if (pageId < 1)
                {
                    return RedirectToActionPreserveMethod();
                }
                return View(products);
            }

            if (groupId == 0 && !string.IsNullOrWhiteSpace((searchKey)))
            {
                ViewData["SearchKey"] = searchKey;
                var result = _context.Products.Where(p => p.Name.Contains(searchKey) || p.Describtion.Contains(searchKey)).Include(p => p.Item)
                    .ToList();
               products.Products = result.OrderBy(p => p.Id).Skip(skip).Take(36).ToList();
                ViewBag.pageCount = result.Count() / 36;
                if (ViewBag.pageCount + 1 < pageId)
                {
                    return Redirect($"/Search/{groupId}/{searchKey}/?PageId={ViewBag.pageCount + 1}");
                }
                if (pageId < 1)
                {
                    return RedirectToActionPreserveMethod();
                }
                return View(products);
            }

            if (groupId == 0)
            {
                if (pageId < 1)
                {
                    return RedirectToActionPreserveMethod();
                }
                products.Products = _context.Products.OrderBy(p => p.Id).Include(p => p.Item).Skip(skip).Take(36).ToList();
                ViewBag.pageCount = (_context.Products.Count()) / 36;
                if (ViewBag.pageCount + 1 < pageId)
                {
                    return RedirectPreserveMethod($"/Shop/{groupId}/?PageId={ViewBag.pageCount + 1}");
                }

                return View(products);
            }

            if (ViewBag.pageCount + 1 < pageId)
            {
                return RedirectPreserveMethod($"/Shop/{groupId}/?PageId={ViewBag.pageCount + 1}");
            }
            if (pageId < 1)
            {
                return RedirectToActionPreserveMethod();
            }
            return View(products);

        }

        [Route("/Group/{id}/{name}/{pageId?}")]//اینکلود برای خروج از لیزی لود و همزمان لود شدن دوتا تیبل با همه
        public IActionResult ShowProductbyGroup(int id, string name, int pageId = 1)//تیبل مربوطه : کتگوری تو پروداکت
        {
            var products = _context.CategoryToProducts
                .Where(c => c.CategoryId == id)
                .Include(c => c.Product).ThenInclude(p => p.Item)
                .Select(c => c.Product).ToList();

            int skip = ((pageId - 1) * 9);
            int count = products.Count();
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
                return RedirectPreserveMethod($"/Group/{id}/{name}/?PageId={ViewBag.pageCount + 1}");
            }

            ViewData["GroupId"] = id;
            ViewData["PageId"] = pageId;
            ViewData["Groupname"] = name;

            var productCount = products.OrderBy(p => p.Id).Skip(skip).Take(9).ToList();
            return View(productCount);
        }




    }
}
