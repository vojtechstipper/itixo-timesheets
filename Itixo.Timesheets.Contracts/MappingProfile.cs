using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Itixo.Timesheets.Shared.Abstractions;

namespace Itixo.Timesheets.Contracts;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        MapperHelper.ApplyMappingsFromAssembly(this);
    }
}

public static class MapperHelper
{
    /// <summary>
    /// Create maps to Automapper profile for IMapFrom<> interface for CallingAssembly.
    /// </summary>
    /// <param name="profile">Profile where map will be created.</param>
    public static void ApplyMappingsFromAssembly(Profile profile)
    {
        ApplyMappingsFromAssembly(profile, Assembly.GetCallingAssembly());
    }

    /// <summary>
    /// Create maps to Automapper profile for IMapFrom<> interface for assembly.
    /// </summary>
    /// <param name="profile">Profile where map will be created.</param>
    public static void ApplyMappingsFromAssembly(Profile profile, Assembly assembly)
    {
        List<Type> types = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
            .ToList();

        foreach (var type in types)
        {
            Type callingType = GetCallingType(type);
            object instance = Activator.CreateInstance(callingType);
            MethodInfo methodInfo = callingType.GetMethod(nameof(IMapFrom<object>.Mapping));
            methodInfo?.Invoke(instance, new object[] { profile });
        }
    }

    private static Type GetCallingType(Type type)
        => type.IsGenericType ? CreateGenericType(type) : type;

    private static Type CreateGenericType(Type type)
    {
        Type argumentType = type.GetGenericArguments()[0];
        if (argumentType.GetInterfaces().Length > 0)
        {
            argumentType = argumentType.GetInterfaces()[0];
        }

        return type.MakeGenericType(argumentType);
    }
}
