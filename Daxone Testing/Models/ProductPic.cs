using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Daxone_Testing.Models
{
    public class ProductPic
    {
        [Key]
        public int PicId { get; set; }
        public string PicName { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
