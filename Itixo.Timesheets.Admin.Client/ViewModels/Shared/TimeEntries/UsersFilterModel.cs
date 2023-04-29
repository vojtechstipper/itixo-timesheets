using Itixo.Timesheets.Admin.Client.ApiServices.TimeEntries;
using Itixo.Timesheets.Contracts.TimeTrackerAccounts;

namespace Itixo.Timesheets.Admin.Client.ViewModels.Shared.TimeEntries;

public interface IUsersFilterModel
{
    List<AccountListContract> Users { get; set; }
    List<AccountListContract> SelectedUsers { get; set; }
    Task LoadUsers();
    List<int> GetSelectedUserIds();
}

public class UsersFilterModel : IUsersFilterModel
{
    private readonly ITimeEntriesApiService timeEntriesApiService;

    public UsersFilterModel(ITimeEntriesApiService timeEntriesApiService)
    {
        this.timeEntriesApiService = timeEntriesApiService;
    }

    public List<AccountListContract> Users { get; set; } = new List<AccountListContract>();
    public List<AccountListContract> SelectedUsers { get; set; } = new List<AccountListContract>();

    public async Task LoadUsers() => Users = (await timeEntriesApiService.GetUsersAsync()).Value.ToList();
    public List<int> GetSelectedUserIds() => SelectedUsers.Select(s => s.Id).ToList();

}
