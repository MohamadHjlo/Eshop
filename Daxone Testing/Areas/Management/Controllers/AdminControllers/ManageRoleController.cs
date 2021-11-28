using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Daxone_Testing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Daxone_Testing.Areas.Management.Controllers.AdminControllers
{
    [Area("Management")]
    [Authorize(Roles = ("Owner"))]
    public class ManageRoleController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public ManageRoleController(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }
        
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View("/Areas/Management/Views/Admin/Users/ManageRole/Index.cshtml", roles);
        }

        [HttpGet]
        public IActionResult AddRole()
        {
            return View("/Areas/Management/Views/Admin/Users/ManageRole/AddRole.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(string roleName,int rank)
        {
            if (string.IsNullOrEmpty(roleName)|| rank==0)
            {
                return NotFound();
            }
            var role = new ApplicationRole(){Name = roleName,RankOfRole = rank};
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("/Areas/Management/Views/Admin/Users/ManageRole/AddRole.cshtml", role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            var result = await _roleManager.DeleteAsync(role);
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View("/Areas/Management/Views/Admin/Users/ManageRole/EditRole.cshtml", role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(string id, string name,int rank)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name) ||rank==0)
            {
                return NotFound();
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            role.Name = name;
            role.RankOfRole=rank;
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View("/Areas/Management/Views/Admin/Users/ManageRole/EditRole.cshtml", role);

        }
    }
}
