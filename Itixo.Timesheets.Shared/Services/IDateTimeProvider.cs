using System;
using System.Globalization;
using Itixo.Timesheets.Shared.Abstractions;

namespace Itixo.Timesheets.Shared.Services;

public interface IDateTimeProvider : IService
{
    public DateTimeOffset Now { get; }
}

public class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset Now => DateTimeOffset.Now;
}

public class CetDateConversionHelper
{
    public const string DisplayFormat = "dd.MM.yyyy HH:mm:ss";
    public static CultureInfo CzechCultureInfo = new CultureInfo("cs-cz");
}
