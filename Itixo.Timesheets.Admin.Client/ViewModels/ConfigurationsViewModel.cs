using DotVVM.Framework.Hosting;
using DotVVM.Framework.Runtime.Filters;
using Itixo.Timesheets.Admin.Client.ApiServices;
using Itixo.Timesheets.Admin.Client.Models.Configurations;
using Itixo.Timesheets.Client.Shared.ApiServices;
using Itixo.Timesheets.Shared.Messaging;
using Itixo.Timesheets.Shared.Resources;
using Itixo.Timesheets.Shared.Services;

namespace Itixo.Timesheets.Admin.Client.ViewModels;

public class ConfigurationsViewModel : MasterPageViewModel
{
    private readonly IConfigurationsApiService configurationsApiService;
    private readonly ISynchronizationApiService synchronizationApiService;
    private readonly ICurrentIdentityProvider currentIdentityProvider;

    public SyncDateRangeModel SyncDateRangeModel { get; set; } = new SyncDateRangeModel();

    public TriggerSyncMessage TriggerSynchronizationDto { get; set; } = new TriggerSyncMessage
    {
        StartDate = DateTime.Now.AddDays(-30),
        EndDate = DateTime.Now
    };

    public ConfigurationsViewModel(IConfigurationsApiService configurationsApiService,
                                   ISynchronizationApiService synchronizationApiService,
                                   ICurrentIdentityProvider currentIdentityProvider,
                                   IDependencies dependencies)
        : base(dependencies)
    {
        this.configurationsApiService = configurationsApiService;
        this.synchronizationApiService = synchronizationApiService;
        this.currentIdentityProvider = currentIdentityProvider;
    }


    public override async Task Init()
    {
        await Context.Authorize(new string[] { "TimeEntries.Administrator" });
        await base.Init();
    }

    public override async Task PreRender()
    {
        await base.PreRender();

        if (!Context.IsPostBack)
        {
            ApiResult<SyncDateRangeModel> syncBusinessDaysApiResult = await configurationsApiService.GetSyncBusinessDaysAsync();

            if (!syncBusinessDaysApiResult.Success)
            {
                DisplayApiResult(syncBusinessDaysApiResult.Validations);
            }
            else
            {
                SyncDateRangeModel = syncBusinessDaysApiResult.Value;
            }
        }
    }

    public async Task SaveStartSyncBussinessDaysFromAsync()
    {
        if (SyncDateRangeModel.StopSyncBusinessDaysAgoValue > SyncDateRangeModel.StartSyncBusinessDaysAgoValue)
        {
            DialogModel.ShowErrorMessageWindow(Validations.SyncBusinessDays_ValidationMessage_Start_Must_Be_Greater_Tan_Stop);
            return;
        }

        ApiResult<SyncDateRangeModel> requestResult = await configurationsApiService.AddOrUpdateSyncBusinessDaysAsync(SyncDateRangeModel);

        DisplayApiResult(requestResult.Validations);
    }

    public async Task TriggerSync()
    {
        var triggerSyncMessage = new TriggerSyncMessage
        {
            StartDate = TriggerSynchronizationDto.StartDate,
            EndDate = TriggerSynchronizationDto.EndDate,
            IdentityExternalId = currentIdentityProvider.ExternalId,
            ManualRun = true
        };

        await synchronizationApiService.RunSynchronizationAsync(triggerSyncMessage);
    }
}

