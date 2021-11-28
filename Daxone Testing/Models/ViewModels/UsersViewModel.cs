using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Daxone_Testing.Models.ViewModels
{
    public class UsersInIndexViewModel
    {
        public IEnumerable<UsersInIndexViewModel> Users { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public IList<string> UserRoles { get; set; }
        public List<ApplicationRole> ValidRoles { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }

    public class AddOrEditUsersViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        [Compare("Password")]
        public string PasswordRepeat { get; set; }
        public List<string> UserRoles { get; set; }
        public List<ApplicationRole> ValidRoles { get; set; }
    }

}
