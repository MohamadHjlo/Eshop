using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Daxone_Testing.Models
{
    public class Product//محصول
    {
       
        public int Id { get; set; }

        public string Name { get; set; }

        public string Describtion { get; set; }
        
        public int ItemId { get; set; }


        public ICollection<CategoryToProduct> CategoryToProducts { get; set; }

        public Item Item { get; set; }

        public ICollection<FavoriteToProducts> FavoriteToProducts { get; set; }

        public List<FactorDetail> Factor { get; set; }

        public ICollection<ProductPic> ProductPics { get; set; }
    }
}
