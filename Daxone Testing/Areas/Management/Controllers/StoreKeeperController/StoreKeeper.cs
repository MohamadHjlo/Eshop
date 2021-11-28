using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Daxone_Testing.Areas.Management.Models.StoreKeeper;
using Daxone_Testing.Data;
using Daxone_Testing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Daxone_Testing.Areas.Management.Controllers.StoreKeeperController
{
    [Area("Management")]
    [Authorize(Roles = ("Owner,Storekeeper"))]
    public class StoreKeeper : Controller
    {
        private DaxoneTestingContext _context;

        public StoreKeeper(DaxoneTestingContext context)
        {
            _context = context;
        }
        public IActionResult Index(int Page = 1, int PageSize = 10)
        {
            var products = new AddEditProductViewModel()
            {
                AllProducts = _context.Products.OrderBy(p => p.Id).Include(p => p.Item).ToList().Skip((Page - 1) * PageSize).Take(PageSize),
                ProductPics = _context.ProductPics.ToList(),
                PageSize = PageSize,
                CurrentPage = Page,
                TotalRecords = _context.Products.Count()
            };



            return View(products);
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            var categories = new AddEditProductViewModel()
            {
                Categories = _context.Category.ToList()
            };
            return View(categories);
        }

        [HttpPost]
        public IActionResult AddProduct(AddEditProductViewModel productView, int catId)
        {
            int picId = 0;
            if (productView.Name == null || productView.Price == null || catId == null || productView.QuantityInStock == null) return Json("nullstates");
            if (productView.Pictures == null) return Json("nullimage");
            var item = new Item()
            {
                Price = productView.Price,
                Discount = productView.Discount,
                QuantityInStuck = productView.QuantityInStock
            };
            _context.Add(item);
            _context.SaveChanges();

            var product = new Product()
            {
                Describtion = productView.Describtion,
                Name = productView.Name,
                Item = item,
            };
            product.ItemId = product.Id;
            _context.SaveChanges();
            _context.Add(product);
            _context.SaveChanges();
            if (productView.Pictures?.Count > 0)
            {
                foreach (var pic in productView.Pictures)
                {
                    var picture = new ProductPic()
                    {
                        ProductId = product.Id
                    };
                    picId++;
                    var ext = Path.GetExtension(pic.FileName);
                    string filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"
                        , "Images/db_Images"
                        , product.Id + $"-{picId}" + Path.ChangeExtension(ext, ".jpg"));
                    picture.PicName = productView.Name;
                    _context.Add(picture);
                    _context.SaveChanges();
                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {
                        pic.CopyTo(stream);
                    }
                }
            }
            _context.CategoryToProducts.Add(new CategoryToProduct()
            {
                CategoryId = catId,
                ProductId = product.Id,
            });
            _context.SaveChanges();
            return Json(true);
        }

        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            ViewBag.piccounter = 0;

            var productViewmodel = _context.Products
                .Include(p => p.Item)
                .Where(p => p.Id == id)
                .Select(s => new AddEditProductViewModel()
                {
                    ID = id,
                    Name = s.Name,
                    Describtion = s.Describtion,
                    QuantityInStock = s.Item.QuantityInStuck,
                    Price = s.Item.Price,
                    ProductPics = _context.ProductPics.Where(p => p.ProductId == id).ToList(),
                    Discount = s.Item.Discount

                }).FirstOrDefault();
            productViewmodel.Categories = _context.Category.ToList();
            productViewmodel.ProductGroups = _context.CategoryToProducts.Where(c => c.ProductId == id)
           .Select(s => s.CategoryId).ToList();
            ViewBag.picnumcount = _context.ProductPics.Count(p => p.ProductId == id);
            return View(productViewmodel);
        }

        [HttpPost]
        public IActionResult EditProduct(AddEditProductViewModel productView, List<int> getPic)
        {
            int picId = 0;
            if (getPic == null) return View();
            var product = _context.Products.Find(productView.ID);
            var item = _context.Items.First(p => p.Id == product.ItemId);

            product.Item.Discount = productView.Discount;
            product.Name = productView.Name;
            product.Describtion = productView.Describtion;
            product.Item.Price = productView.Price;
            product.Item.QuantityInStuck = productView.QuantityInStock;
            _context.SaveChanges();

            if (productView.Pictures?.Count > 0)
            {
                foreach (var pic in productView.Pictures)
                {
                    var picture = new ProductPic()
                    {
                        ProductId = product.Id,
                        PicName = productView.Name
                    };
                    picId = productView.AddedImages;
                    picId++;
                    var ext = Path.GetExtension(pic.FileName);
                    string filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"
                        , "Images/db_Images"
                        , product.Id + $"-{picId}" + Path.ChangeExtension(ext, ".jpg"));
                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {
                        pic.CopyTo(stream);
                    }
                    foreach (var prpics in getPic)
                    {
                        if (_context.ProductPics.Any(p => p.PicId == prpics))
                        {
                            var previousState = _context.ProductPics.FirstOrDefault(c => c.PicId == prpics);
                            _context.ProductPics.Remove(previousState);
                        }
                    }
                    _context.Add(picture);
                    _context.SaveChanges();
                }
            }

            _context.CategoryToProducts.Where(c => c.ProductId == product.Id).ToList()
                .ForEach(g => _context.CategoryToProducts.Remove(g));
            if (productView.ProductGroups.Any() && productView.ProductGroups.Count >= 0)
            {
                foreach (var gr in productView.ProductGroups)
                {
                    _context.CategoryToProducts.Add(new CategoryToProduct()
                    {
                        CategoryId = gr,
                        ProductId = product.Id,
                    });
                }
            }
            return Json(true);
        }

        [HttpPost]
        public IActionResult EditPictures(int Id, int numInDirectory, int productId)
        {
            var c = _context.ProductPics.Where(p => p.ProductId == productId).ToList();

            var picture = _context.ProductPics.Find(Id);
            _context.ProductPics.Remove(picture);
            string filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"
                , "Images/db_Images"
                , productId + $"-{numInDirectory}" + ".jpg");

            if (System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
                foreach (var item in c)
                {
                    string nextPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"
                        , "Images/db_Images", $"{productId}");

                    string path = nextPath + $"-{numInDirectory + 1}" + ".jpg";
                    string replacepath = nextPath + $"-{numInDirectory}" + ".jpg";
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Move(path, replacepath);
                    }
                    numInDirectory++;
                }
            }
            _context.SaveChanges();
            return Json(true);
        }



        [HttpPost]
        public IActionResult ModalQuantity(int ID, int quantity)
        {
            var product = _context.Products.Find(ID);
            var item = _context.Items.First(p => p.Id == product.ItemId);
            product.Item.QuantityInStuck = quantity;
            _context.SaveChanges();
            return Json("successEdit");
        }

        [HttpPost]
        public IActionResult ModalEdit(int ID, float discount)
        {
            var product = _context.Products.Find(ID);
            var item = _context.Items.First(p => p.Id == product.ItemId);
            product.Item.Discount = discount;
            _context.SaveChanges();
            return Json("successEdit");
        }

        [HttpPost]
        public IActionResult DeleteProduct(AddEditProductViewModel productView)
        {
            int picnum = 0;
            var product = _context.Products.Find(productView.ID);
            var item = _context.Items.First(p => p.Id == product.ItemId);
            _context.Items.Remove(item);
            _context.Products.Remove(product);
            var pictures = _context.ProductPics.Where(p => p.ProductId == productView.ID).ToList();
            foreach (var pic in pictures)
            {
                _context.Remove(pic);
                picnum++;
                string filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"
                    , "Images/db_Images"
                    , product.Id + $"-{picnum}" + ".jpg");
                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                }
            }
            _context.SaveChanges();

            return Json(true);
        }

    }

}

