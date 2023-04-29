﻿using Itixo.Timesheets.Shared.ConstantObjects;
using Itixo.Timesheets.Shared.Roles;
using Itixo.Timesheets.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Itixo.Timesheets.Application.Services.Shared;

public class CurrentIdentityProvider : ICurrentIdentityProvider
{
    private readonly List<RoleDefinition> roleDefinitions;

    public string ExternalId { get; set; }
    public List<Claim> RoleClaims { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }

    public CurrentIdentityProvider(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        HttpContext httpContext = httpContextAccessor.HttpContext;
        roleDefinitions = configuration.GetSection("AppRoles").Get<List<RoleDefinition>>();

        if (httpContext == null)
        {
            return;
        }

        if (IsIdentityDaemon(httpContext))
        {
            ExternalId = httpContext.User.Claims.FirstOrDefault(f => f.Type == ExpandingClaimTypes.AppId)?.Value;
        }
        else
        {

            ExternalId = httpContext.User.Claims.FirstOrDefault(f => f.Type == ExpandingClaimTypes.ObjectIdClaimType)?.Value;
            RoleClaims = httpContext.User.Claims.Where(f => f.Type == ClaimTypes.Role).ToList();
            Email = httpContext.User.Claims.FirstOrDefault(f => f.Type == ClaimTypes.Name)?.Value;
            Username = httpContext.User.Claims.FirstOrDefault(f => f.Type == ClaimTypes.Name)?.Value;
        }
    }

    private bool IsIdentityDaemon(HttpContext httpContext)
    {
        return httpContext.User.FindAll(ClaimTypes.Role).Select(role => role.Value).Contains("access_as_sync_application");
    }

    public bool IsAdmin()
    {
        string adminRoleName = roleDefinitions?.FirstOrDefault(f => f.Key == RoleDefinition.AdminRoleKey)?.RoleName;

        if (adminRoleName == null)
        {
            return false;
        }

        return RoleClaims?.Any(claim => claim.Value == adminRoleName) ?? false;
    }

    public bool IsTimeSheetApp()
    {
        string timesheetAppRoleName = roleDefinitions?.FirstOrDefault(f => f.Key == RoleDefinition.TimeSheetAppKey)?.RoleName;

        if (timesheetAppRoleName == null)
        {
            return false;
        }

        return RoleClaims?.Any(claim => claim.Value == timesheetAppRoleName) ?? false;
    }
}
