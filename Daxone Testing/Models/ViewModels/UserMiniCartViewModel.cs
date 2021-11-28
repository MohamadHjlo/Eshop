using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Daxone_Testing.Models.ViewModels
{
    public class UserMiniCartViewModel
    {
        public List<List<FactorDetail>> FactorDitailsList { get; set; }
        public List<Factor> Factors { get; set; }
    }
}
