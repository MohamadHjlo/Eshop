using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Daxone_Testing.Areas.Management.Controllers
{
    [Area("Management")]
    
    public class HomeController : Controller
    {
        
        public string Index()
        {
            return "This page name is admin";
        }

    }
}
