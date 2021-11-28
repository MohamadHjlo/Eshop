using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Daxone_Testing.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Describtion { get; set; }

        public ICollection<CategoryToProduct> CategoryToProducts { get; set; }

        public ICollection<CategoryAttr> CategoryAttrs { get; set; }
    }
}
