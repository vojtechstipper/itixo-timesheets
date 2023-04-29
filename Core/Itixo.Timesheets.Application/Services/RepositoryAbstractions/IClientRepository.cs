using System.Collections.Generic;
using System.Threading.Tasks;
using Itixo.Timesheets.Domain;

namespace Itixo.Timesheets.Application.Services.RepositoryAbstractions;

public interface IClientRepository : IEntityRepository<Client, int>
{
    Task<List<T>> GetAllAsync<T>();
}
