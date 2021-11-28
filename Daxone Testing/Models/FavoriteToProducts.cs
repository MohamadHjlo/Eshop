using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Daxone_Testing.Models
{
    public class FavoriteToProducts
    {
        public FavoriteToProducts ()
        {
            UserFavorites=new UserFavorites();
        }

        public int UserFavoritesId { get; set; }

        public int ProductId { get; set; }


        public UserFavorites UserFavorites { get; set; }

        public Product Product { get; set; }
    }
}
