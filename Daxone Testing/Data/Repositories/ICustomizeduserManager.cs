using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Daxone_Testing.Models;
using Microsoft.AspNetCore.Identity;

namespace Daxone_Testing.Data.Repositories
{
    interface ICustomizeduserManager
    {
        public Task<List<string>> GetUserRoles(string userId);
    }
    public class CustomizeduserManager : ICustomizeduserManager
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<ApplicationRole> _roleManager;
        public CustomizeduserManager(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<List<string>> GetUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
           var role= await _userManager.GetRolesAsync(user);
           return role.ToList();
        }
    }
}
