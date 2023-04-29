using System;
using System.Linq;

namespace Itixo.Timesheets.Shared.Extensions;

public static class TimeSpanExtensions
{
    public static string ToCustomHoursFormat(this TimeSpan duration) => $"{(int)duration.TotalHours:00}:{duration.Minutes:00}:{duration.Seconds:00} h";
    public static TimeSpan FromCustomHoursFormat(this string duration)
    {
        if (duration.IndexOf(':') == -1 || string.IsNullOrWhiteSpace(duration))
        {
            return TimeSpan.Zero;
        }

        var hours = TimeSpan.FromHours(double.Parse(duration.Substring(0, duration.IndexOf(':'))));
        var minutes = TimeSpan.FromMinutes(double.Parse(duration.Substring(duration.IndexOf(':')+1, 2)));
        return hours.Add(minutes);
    }

    public static TimeSpan Sum(params TimeSpan[] times)
    {
        return new TimeSpan(times.Select(s => s.Ticks).Sum());
    }
}
