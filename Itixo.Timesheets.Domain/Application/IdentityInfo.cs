using System.Collections.Generic;
using Itixo.Timesheets.Domain.TimeTrackers;
using Itixo.Timesheets.Shared.Abstractions;

namespace Itixo.Timesheets.Domain.Application;

public class IdentityInfo : IEntity<int>
{
    public int Id { get; set; }
    public string ExternalId { get; set; }
    public string Identifier { get; set; }
    public string Email { get; set; }
    public virtual ICollection<IdentityTimeTrackerAccount> Accounts { get; set; } = new List<IdentityTimeTrackerAccount>();

    public static IdentityInfo From(string externalId, string username)
    {
        return new IdentityInfo { ExternalId = externalId, Identifier = username };
    }
}
