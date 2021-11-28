
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Daxone_Testing.Models.ViewModels
{
    public class AddUserToRoleViewModel
    {
        #region Constructors
        public AddUserToRoleViewModel()
        {
            UserRoles = new List<UserRolesViewModel>();
        }

        public AddUserToRoleViewModel(string userId, List<UserRolesViewModel> userRoles)
        {
            UserId = userId;
            UserRoles = userRoles;
        }

        #endregion
        public string UserId { get; set; }
        public List<UserRolesViewModel> UserRoles { get; set; }
    }

    public class UserRolesViewModel
    {
        #region cunstructors

        public UserRolesViewModel()
        {
        }

        public UserRolesViewModel(string roleName)
        {
            RoleName = roleName;
        }
        #endregion
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}


