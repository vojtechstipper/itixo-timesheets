using System;
using System.Collections.Generic;
using System.Linq;
using Itixo.Timesheets.Domain.Application;
using Itixo.Timesheets.Shared.Abstractions;

namespace Itixo.Timesheets.Domain.TimeTrackers;

public class TimeTrackerAccount : IEntity<int>
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string ExternalId { get; set; }
    public TimeTracker TimeTracker { get; set; }

    public virtual ICollection<IdentityTimeTrackerAccount> Identities { get; set; } = new List<IdentityTimeTrackerAccount>();
    public IList<ExternalIdChange> ExternalIdHistory { get; set; } = new List<ExternalIdChange>();

    public void AssignIdentityToAccount(IdentityInfo identityInfo) =>
        Identities.Add(
            new IdentityTimeTrackerAccount
            {
                TimeTrackerAccount = this,
                IdentityInfo = identityInfo,
                TimeTrackerAccountId = Id,
                IdentityInfoId = identityInfo.Id
            });

    private void ExternalIdChanged(string ip)
    {
        List<ExternalIdChange> externalIdHistory = ExternalIdHistory?.ToList() ?? new List<ExternalIdChange>();
        externalIdHistory.Add(new ExternalIdChange { ExternalId = ExternalId, Ip = ip});
        ExternalIdHistory = externalIdHistory;
    }

    public static TimeTrackerAccount Create(ITimeTrackerAccountModifiable parameters)
    {
        var account = new TimeTrackerAccount
        {
            ExternalId = parameters.ExternalId,
            Username = parameters.Username,
            TimeTracker = new TimeTracker { Id = parameters.TimeTrackerId }
        };
        account.ExternalIdChanged(parameters.Ip);
        var identityInfo = IdentityInfo.From(parameters.CurrentIdentityExternalId, parameters.Username);
        account.AssignIdentityToAccount(identityInfo);

        return account;
    }

    public void Update(ITimeTrackerAccountModifiable parameters)
    {
        if (ExternalId != parameters.ExternalId)
        {
            ExternalIdChanged(parameters.Ip);
        }

        IdentityInfo identityInfo = Identities?.Select(s => s.IdentityInfo)
            .FirstOrDefault(ii => ii.ExternalId == parameters.CurrentIdentityExternalId);

        if (identityInfo == null)
        {
            identityInfo = IdentityInfo.From(parameters.CurrentIdentityExternalId, parameters.Username);
            AssignIdentityToAccount(identityInfo);
        }

        ExternalId = parameters.ExternalId;
        Username = parameters.Username;
        TimeTracker = new TimeTracker { Id = parameters.TimeTrackerId };
    }

    public interface ITimeTrackerAccountModifiable
    {
        int Id { get; set; }
        string Username { get; set; }
        string ExternalId { get; set; }
        string Ip { get; set; }
        string CurrentIdentityExternalId { get; set; }
        int TimeTrackerId { get; set; }
    }
}

public class ExternalIdChange
{
    public string Ip { get; set; }
    public string ExternalId { get; set; } = "";
    public DateTime CreateDate { get; set; } = DateTime.Now;
}
