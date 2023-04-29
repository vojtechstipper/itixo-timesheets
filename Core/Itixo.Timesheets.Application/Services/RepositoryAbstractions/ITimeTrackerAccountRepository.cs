using System.Collections.Generic;
using System.Threading.Tasks;
using Itixo.Timesheets.Contracts.TimeTrackerAccounts;
using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Domain.TimeTrackers;

namespace Itixo.Timesheets.Application.Services.RepositoryAbstractions;

public interface ITimeTrackerAccountRepository : IEntityRepository<TimeTrackerAccount, int>
{
    Task<IEnumerable<T>> ListAsync<T>();
    Task<List<TimeTrackerAccount>> RawListAsync();
    Task<TimeTrackerAccount> GetAsync(int id);
    Task<AccountDetailContract> GetUsersTogglAccountDetailsAsync(string username);
    Task<int> InsertAsync(TimeTrackerAccount timeTrackerAccount);
    Task UpdateAsync(TimeTrackerAccount timeTrackerAccount);
    Task DeleteAsync(int? id);
    Task<bool> AnyAsync(int id);
    Task<TimeTrackerAccount> UnauthorizedGetByParamsAsync(string username, string externalId);
    Task<TimeTrackerAccount> UnauthorizedGetAsync(int id);
    Task<IEnumerable<T>> OnlyApplicationAccountsAsync<T>();
}
