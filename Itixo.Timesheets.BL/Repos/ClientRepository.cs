using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Itixo.Timesheets.Infrastructure.Repos;

public class ClientRepository : AppRepositoryBase<Client, int>, IClientRepository
{
    private readonly IMapper mapper;
    public ClientRepository(IDbContext dbContext, IMapper mapper) : base(dbContext) {
        this.mapper = mapper;
    }

    public Task<List<T>> GetAllAsync<T>() => mapper.ProjectTo<T>(GetDbSet()).ToListAsync();
}
