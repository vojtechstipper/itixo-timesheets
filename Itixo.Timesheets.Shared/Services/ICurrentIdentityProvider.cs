using System.Collections.Generic;
using System.Security.Claims;

namespace Itixo.Timesheets.Shared.Services;

public interface ICurrentIdentityProvider
{
    string ExternalId { get; set; }
    List<Claim> RoleClaims { get; set; }
    string Email { get; set; }
    string Username { get; set; }
    bool IsAdmin();
    bool IsTimeSheetApp();
}
