using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace Daxone_Testing.Models
{
    public class FactorDetail//ریز فاکتور
    {
        [Key]
        public int DetailId { get; set; }
        [Required]
        public int FactorId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Count { get; set; }

        public Factor Factors { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

    }
}
