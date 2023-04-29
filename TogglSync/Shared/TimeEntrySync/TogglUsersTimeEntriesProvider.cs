using Itixo.Timesheets.Shared.Abstractions;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogglSyncShared.DataObjects;
using TogglSyncShared.Refit;

namespace TogglSyncShared.TimeEntrySync;

public interface ITogglUsersTimeEntriesProvider : IService
{
    Task<List<TogglTimeEntry>> GetWhereInWorkspaceIdsAsync(TogglUsersTimeEntryParams timeEntryParams);
}

public class TogglUsersTimeEntriesProvider : ITogglUsersTimeEntriesProvider
{
    public async Task<List<TogglTimeEntry>> GetWhereInWorkspaceIdsAsync(TogglUsersTimeEntryParams timeEntryParams)
    {
        if (!timeEntryParams.WorkspaceIds.Any())
        {
            throw new Exception("Missing WorkspaceId in database");
        }

        try
        {
            var authHeader = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(timeEntryParams.ApiToken + ":" + "api_token"));

            var refitSettings = new RefitSettings()
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(authHeader)
            };
            var togglAPI = RestService.For<ITogglAPI>("https://api.track.toggl.com/api/v9", refitSettings);

            var parameters = new { start_date = timeEntryParams.GetStartParameter(), end_date = timeEntryParams.GetEndIsoDate() };
            List<TogglTimeEntry> togglTimeEntries = await togglAPI.GetTimeEntries(parameters);

            return togglTimeEntries.Where(w => timeEntryParams.WorkspaceIds.Contains(w.WorkspaceId)).ToList();
        }
        catch (Exception e)
        {
            return new List<TogglTimeEntry>();
        }
    }
}
