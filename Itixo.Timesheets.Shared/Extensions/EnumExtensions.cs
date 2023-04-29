﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Itixo.Timesheets.Shared.Extensions;

public static class EnumExtensions
{
    public static string GetEnumDescription(this Enum value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString());

        if (fi.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Any())
        {
            return attributes.First().Description;
        }

        return value.ToString();
    }
}
