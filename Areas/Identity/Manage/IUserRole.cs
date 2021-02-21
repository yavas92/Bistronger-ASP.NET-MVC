using Bistronger.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Abdullah
/// </summary>
namespace Bistronger.Areas.Identity.Manage
{
    public interface IUserRole
    {
        void CreateRoleIfNotExists();
        void AssignRoleToUser(ApplicationUser user, UserRoleType userRole);
    }
}
