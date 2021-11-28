using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Daxone_Testing.Data;
using Daxone_Testing.Data.Repositories;
using Daxone_Testing.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Daxone_Testing.Components
{
    public class CartComponent : ViewComponent
    {
        private IUserRepository _userRepository;
        private DaxoneTestingContext _context;


        public CartComponent(IUserRepository userRepository,DaxoneTestingContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }
        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user =  User as ClaimsPrincipal;
            return  View("/Views/Components/User/CartViewComponent.cshtml",(
                _userRepository.GetMiniCartfUser(user.FindFirstValue(ClaimTypes.NameIdentifier))
                ));
        }
        
    }
}
