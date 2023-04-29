using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Itixo.Timesheets.Shared.Roles;

public class RolesAuthorizeAttribute : AuthorizeAttribute
{
    public RolesAuthorizeAttribute(string[] roles)
    {
        Roles = string.Join(",", roles);
    }
}
