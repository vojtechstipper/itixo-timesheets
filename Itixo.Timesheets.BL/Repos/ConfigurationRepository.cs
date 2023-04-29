using AutoMapper;
using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Domain.Application;
using Itixo.Timesheets.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Itixo.Timesheets.Shared.ConstantObjects;
using Task = System.Threading.Tasks.Task;

namespace Itixo.Timesheets.Infrastructure.Repos;

public class ConfigurationRepository : AppRepositoryBase<Configuration, int>, IConfigurationRepository
{
    private readonly IMapper mapper;

    public ConfigurationRepository(IDbContext dbContext, IMapper mapper) : base(dbContext)
    {
        this.mapper = mapper;
    }

    public async Task AddOrUpdateSyncHourlyRepetitionAsync(int syncHourlyRepetition)
    {
        if (syncHourlyRepetition < 1 || syncHourlyRepetition > 12)
        {
            throw new ArgumentException("Value SyncHourlyRepetition must be in range 1 to 12");
        }

        Configuration configuration = await GetByKeyAsync(ConfigurationConstants.SyncHourlyRepetition);

        string syncHourlyRepetitionString = syncHourlyRepetition.ToString();

        if (configuration == null)
        {
            await InsertConfiguration(ConfigurationConstants.SyncHourlyRepetition, syncHourlyRepetitionString);
            return;
        }

        if (configuration.Value != syncHourlyRepetitionString)
        {
            configuration.Value = syncHourlyRepetitionString;
            await UpdateConfiguration(configuration);
        }
    }

    public async Task<bool> IsUniqueAsync(string key) => (await dbContext.Configurations.CountAsync(a => a.Key == key)) <= 1;

    public Configuration InitializeConfiguration(string key, string value) => new Configuration { Key = key, Value = value };

    public async Task<Configuration> GetByKeyAsync(string key, CancellationToken token = default)
        => await dbContext.Configurations.FirstOrDefaultAsync(c => c.Key == key, token);

    public async Task<bool> AnyAsync(int id, CancellationToken token = default)
        => await dbContext.Configurations.AnyAsync(a => a.Id == id, token);

    public Task<int> InsertConfiguration(string key, string value) => InsertConfiguration(InitializeConfiguration(key, value));

    public async Task<int> InsertConfiguration(Configuration configuration)
    {
        await dbContext.Configurations.AddAsync(configuration);
        await dbContext.SaveChangesAsync();
        return configuration.Id;
    }

    public async Task UpdateConfiguration(Configuration configuration)
    {
        dbContext.Configurations.Update(configuration);
        await dbContext.SaveChangesAsync();
    }
}
