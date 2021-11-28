using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Daxone_Testing.Models;
using Microsoft.AspNetCore.Http;

namespace Daxone_Testing.Areas.Management.Models.StoreKeeper
{
    public class AddEditProductViewModel
    {   
        public int ID { get; set; }
            
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Describtion { get; set; }

        public int QuantityInStock { get; set; }

        public List<IFormFile> Pictures { get; set; }//for upload all type of files

        public List<Category> Categories { get; set; }

        public int AddedImages { get; set; }

        public List<int> ProductGroups { get; set; }

        public List<ProductPic> ProductPics { get; set; }
        public Product Product { get; set; }

        public float Discount { get; set; }
        public IEnumerable<Product> AllProducts { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public int TotalRecords { get; set; }
    }
    
}
