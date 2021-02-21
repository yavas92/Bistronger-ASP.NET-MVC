using Bistronger.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bistronger.Areas.Identity
{
    public class AuthorizeRolesAttribute: AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params UserRoleType[] allowedRoles):base()
        {
            var allowedRolesAsStrings = allowedRoles.Select(x => Enum.GetName(typeof(UserRoleType), x));
            Roles = string.Join(",", allowedRolesAsStrings);
        }
    }
}
