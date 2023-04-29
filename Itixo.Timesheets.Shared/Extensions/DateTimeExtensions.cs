using System;
using System.Linq;
using DayOfWeek = System.DayOfWeek;

namespace Itixo.Timesheets.Shared.Extensions;

public static class DateTimeExtensions
{
    public static DateTimeOffset GetDateWithMaximumTime(this DateTimeOffset input)
    {
        return input.AddDays(1).GetDateWithMinimumTime();
    }

    public static DateTimeOffset GetDateWithMinimumTime(this DateTimeOffset input)
    {
        return input.Date;
    }

    public static DateTime GetDateWithMaximumTime(this DateTime input)
    {
        return input.AddDays(1).GetDateWithMinimumTime();
    }

    public static DateTime GetDateWithMinimumTime(this DateTime input)
    {
        return input.Date;
    }

    public static DateTime AddBusinessDays(this DateTime originalDate, int workDays)
    {
        if (workDays == 0)
        {
            return originalDate;
        }

        for (int i = 0; i < Math.Abs(workDays); i++)
        {
            originalDate = originalDate.AddDays(workDays < 0 ? -1 : +1);

            if (originalDate.IsHoliday() || originalDate.DayOfWeek == DayOfWeek.Saturday || originalDate.DayOfWeek == DayOfWeek.Sunday)
            {
                if (workDays < 0)
                {
                    --workDays;
                }
                else
                {
                    ++workDays;
                }
            }
        }

        return originalDate;
    }

    public static bool IsHoliday(this DateTime originalDate)
    {
        var originalDateMidnight = new DateTime(originalDate.Year, originalDate.Month, originalDate.Day);

        DateTime[] holidayDays = new[]
        {
            new DateTime(DateTime.Now.Year, 1, 1),
            new DateTime(DateTime.Now.Year, 5, 1),
            new DateTime(DateTime.Now.Year, 5, 8),
            new DateTime(DateTime.Now.Year, 7, 5),
            new DateTime(DateTime.Now.Year, 7, 6),
            new DateTime(DateTime.Now.Year, 9, 28),
            new DateTime(DateTime.Now.Year, 10, 28),
            new DateTime(DateTime.Now.Year, 11, 17),
            new DateTime(DateTime.Now.Year, 12, 24),
            new DateTime(DateTime.Now.Year, 12, 25),
            new DateTime(DateTime.Now.Year, 12, 26),
            new DateTime(DateTime.Now.Year, 12, 31)
        };

        return holidayDays.Contains(originalDateMidnight);
    }

    public static DateTime RandomDate()
    {
        var gen = new Random();
        var start = new DateTime(1995, 1, 1);
        int range = (DateTime.Today - start).Days;
        return start.AddDays(gen.Next(range));
    }
}
