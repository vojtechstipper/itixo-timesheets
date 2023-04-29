using System;
using System.Collections.Generic;

namespace TogglAPI.NetStandard.QueryObjects;

public class TimeEntryParams
{
    public DateTime? StartDate { get;  set; }
    public DateTime? EndDate { get; set; }
    public List<string> TagNames { get; set; }
    public decimal? ProjectId { get; set; }

    public TimeEntryParams()
    {
        TagNames = new List<string>();
    }

    public List<KeyValuePair<string,string>>GetParameters()
    {
        var lst = new List<KeyValuePair<string, string>>();
        if(StartDate.HasValue)
        lst.Add(new KeyValuePair<string, string>("start_date", GetStartParameter()));
        if (EndDate.HasValue)
        lst.Add(new KeyValuePair<string, string>("end_date", GetEndIsoDate()));
        return lst;
    }
    public string GetStartParameter()
    {
        return GetIsoDate(StartDate);
    }
    public string GetEndIsoDate()
    {
        return GetIsoDate(EndDate);
    }
    private string GetIsoDate(DateTime? dt)
    {
        return dt.GetValueOrDefault().ToString("yyyy-MM-ddTHH:mm:sszzz");
    }
}
