using System.Collections.Generic;

namespace Daxone_Testing.Models.ViewModels
{
    public class DetailViewModel
    {
        public Product Product { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        public IEnumerable<ProductPic> ProductPics { get; set; }

        public List<Product> UserFavProducts { get; set; }
    }
}
