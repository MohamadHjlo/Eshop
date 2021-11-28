using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Daxone_Testing.Models
{
    public class ApplicationRole:IdentityRole
    {
        public int RankOfRole { get; set; }
    }
}
