using Itixo.Timesheets.Shared.Abstractions;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TogglSyncShared.DataObjects;
using TogglSyncShared.Refit;

namespace TogglSyncShared;

public interface ITogglUsersWorkspacesProvider : IService
{

    Task<IEnumerable<Workspace>> GetUsersWorkspacesAsync(string userApiToken);
}

public class TogglUsersWorkspacesProvider : ITogglUsersWorkspacesProvider
{
    public async Task<IEnumerable<Workspace>> GetUsersWorkspacesAsync(string userApiToken)
    {

        var authHeader = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(userApiToken + ":" + "api_token"));

        var refitSettings = new RefitSettings()
        {
            AuthorizationHeaderValueGetter = () => Task.FromResult(authHeader)
        };
        var togglAPI = RestService.For<ITogglAPI>("https://api.track.toggl.com/api/v9", refitSettings);

        return await togglAPI.GetWorkspaces();
    }
}
