using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Daxone_Testing.Models
{
    public class Factor
    {
        [Key]
        public int FactorId { get; set; }

        [Required]  
        public string UserId { get; set; }

        [Required]
        public DateTime CreatDate { get; set; }

        public bool IsFinaly { get; set; }

      //**********************************************\\
       
        
        public  ApplicationUser Users { get; set; }//relation

        public List<FactorDetail> FactorDetails { get; set; }
    }
}
