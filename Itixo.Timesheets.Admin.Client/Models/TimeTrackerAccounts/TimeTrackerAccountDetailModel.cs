using DotVVM.Framework.ViewModel;
using Itixo.Timesheets.Contracts.TimeTrackers;
using Itixo.Timesheets.Shared.Resources;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Itixo.Timesheets.Admin.Client.Models.TimeTrackerAccounts;

public class TimeTrackerAccountDetailModel
{
    public TimeTrackerAccountDetailModel()
    {
        Username = "";
        ExternalId = "";
        Ip = "";
        Email = "";
    }

    public int Id { get; set; }

    [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "UserDetailModel_ValidationMessage_Name_Of_Account_Is_Required")]
    [JsonProperty("username")]
    public string Username { get; set; }

    public string ExternalId { get; set; }
    public string Email { get; set; }
    public int TimeTrackerId => TimeTrackerContract?.Id ?? 0;
    public TimeTrackerContract TimeTrackerContract { get; set; }

    public string Ip { get; set; }

    [Bind(Direction.ServerToClient)]
    public bool IsEmpty => ExternalId == Empty.ExternalId && Username == Empty.Username;

    public static TimeTrackerAccountDetailModel Empty => new TimeTrackerAccountDetailModel { Username = "", ExternalId = "" };

    public static TimeTrackerAccountDetailModel From(TimeTrackerAccountGridModel from)
    {
        return new TimeTrackerAccountDetailModel
        {
            Id = from.Id,
            Username = from.Username,
            ExternalId = from.ExternalId,
            TimeTrackerContract = from.TimeTracker,
            Email = from.Email
        };
    }
}
