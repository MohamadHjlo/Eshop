using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Daxone_Testing.Models
{
    public class ApplicationUser:IdentityUser
    {
        public IEnumerable<Factor> Factors { get; set; }
    }
}
