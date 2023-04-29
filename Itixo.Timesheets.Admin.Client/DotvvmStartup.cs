using DotVVM.Framework.Configuration;
using DotVVM.Framework.Controls.Bootstrap4;
using DotVVM.Framework.ResourceManagement;
using DotVVM.Framework.Routing;
using Itixo.Timesheets.Admin.Client.Configurations;
using Itixo.Timesheets.Admin.Client.Filters;

namespace Itixo.Timesheets.Admin.Client;

public class DotvvmStartup : IDotvvmStartup, IDotvvmServiceConfigurator
{
    public void Configure(DotvvmConfiguration config, string applicationPath)
    {
        config.AddBootstrap4Configuration();

        ConfigureRoutes(config);
        ConfigureControls(config);
        ConfigureResources(config);

        config.DefaultCulture = "cs-CZ";

        config.Runtime.GlobalFilters.Add(new ExceptionFilter());
    }

    private void ConfigureRoutes(DotvvmConfiguration config)
    {
        config.RouteTable.Add(RouteNames.Error, "500", "Views/Common/Error.dothtml");
        config.RouteTable.Add(RouteNames.NotFound, "404", "Views/Common/NotFound.dothtml");
        config.RouteTable.Add(RouteNames.Unauthorized, "access-denied", "Views/Common/Unauthorized.dothtml");

        config.RouteTable.Add(RouteNames.Default, "", "Views/TimeEntries/TimeEntries.dothtml");
        config.RouteTable.Add(RouteNames.Account, "muj-ucet", "Views/Account.dothtml");
        config.RouteTable.Add(RouteNames.TimeTrackerAccounts, "ucty", "Views/TimeTrackerAccounts.dothtml");
        config.RouteTable.Add(RouteNames.TimeEntries, "zaznamy", "Views/TimeEntries/TimeEntries.dothtml");
        config.RouteTable.Add(RouteNames.Configurations, "synchronizator", "Views/Configurations.dothtml", new { Id = 0 });
        config.RouteTable.Add(RouteNames.SyncHistory, "historie-synchronizace", "Views/SynchronizationHistory.dothtml");
        config.RouteTable.Add(RouteNames.Reports, "reporty", "Views/Reports.dothtml");
        config.RouteTable.Add(RouteNames.Workspaces, "workspaces", "Views/Workspaces.dothtml");
        config.RouteTable.Add(RouteNames.AddTimeEntry, "pridat-zaznam", "Views/AddTimeEntry.dothtml");
        config.RouteTable.Add(RouteNames.PreDeleteTimeEntries, "zaznamy-ke-smazani", "Views/TimeEntries/PreDeleteTimeEntries.dothtml");
        config.RouteTable.AutoDiscoverRoutes(new DefaultRouteStrategy(config));
    }

    private void ConfigureControls(DotvvmConfiguration config)
    {
        config.Markup.AddMarkupControl("my", "TimeEntryVersionsWindow", "Views/TimeEntries/TimeEntryVersionsWindow.dotcontrol");
        config.Markup.AddMarkupControl("my", "TimeEntryStateChangesWindow", "Views/TimeEntries/TimeEntryStateChangesWindow.dotcontrol");
    }

    private void ConfigureResources(DotvvmConfiguration config)
    {
        config.Resources.Register("stylesheet-css", new StylesheetResource()
        {
            Location = new UrlResourceLocation("StyleSheet.css")
        });
    }

    public void ConfigureServices(IDotvvmServiceCollection options)
    {
        options.AddDefaultTempStorages("temp");
        options.AddBusinessPack();
    }
}
