using System.Collections.Generic;

namespace Itixo.Timesheets.Shared.Abstractions;

public interface ITogglWorkspaceService<T>
where T : class, ITogglWorkspaceDto, new()
{
    System.Threading.Tasks.Task<List<T>> List();
    ITogglWorkspaceService<T> Create(string apiToken);
}
