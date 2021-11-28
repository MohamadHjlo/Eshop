using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Daxone_Testing.Models
{
    public class UserFavorites
    {
        public int Id { get; set; }
        
        public string UserId { get; set; }

        public ICollection<FavoriteToProducts> FavoriteToProducts { get; set; }

        public ApplicationUser User { get; set; }
    }
}