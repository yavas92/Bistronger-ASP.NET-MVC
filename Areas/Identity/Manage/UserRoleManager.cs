using Bistronger.Data;
using Bistronger.Data.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Abdullah
/// </summary>
namespace Bistronger.Areas.Identity.Manage
{
    public class UserRoleManager : IUserRole
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRoleManager(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void AssignRoleToUser(ApplicationUser user, UserRoleType userRole)
        {

            if (!_userManager.IsInRoleAsync(user, userRole.ToString()).Result)
                _userManager.AddToRoleAsync(user, userRole.ToString()).Wait();
        }

        public void CreateRoleIfNotExists()
        {
            foreach (UserRoleType userRole in (UserRoleType[])Enum.GetValues(typeof(UserRoleType)))
            {
                if (!_roleManager.RoleExistsAsync(userRole.ToString()).Result)
                {
                    _roleManager.CreateAsync(new IdentityRole(userRole.ToString())).Wait();
                }
            }
        }
    }
}
