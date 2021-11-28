using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Daxone_Testing.Models.ViewModels
{
    public class MyAccountViewmodel
    {
        public ApplicationUser User { get; set; }

        public IEnumerable<Factor> UserFactor { get; set; }

        public UserFavorites UserFavorites { get; set; }

        public List<Product> UserProducts { get; set; }
    }   
}
