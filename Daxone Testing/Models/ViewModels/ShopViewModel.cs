using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Daxone_Testing.Models.ViewModels
{
    public class ShopViewModel
    {
        public List<Product> Products { get; set; }

        public UserFavorites UserFavorites { get; set; }

        public List<Product> UserFavProducts { get; set; }
    }
}
