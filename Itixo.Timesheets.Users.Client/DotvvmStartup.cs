using DotVVM.Framework.Configuration;
using DotVVM.Framework.Controls.Bootstrap4;
using DotVVM.Framework.Routing;
using Itixo.Timesheets.Users.Client.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Itixo.Timesheets.Users.Client;

public class DotvvmStartup : IDotvvmStartup, IDotvvmServiceConfigurator
{
    // For more information about this class, visit https://dotvvm.com/docs/tutorials/basics-project-structure
    public void Configure(DotvvmConfiguration config, string applicationPath)
    {
        config.AddBootstrap4Configuration();

        ConfigureRoutes(config, applicationPath);
        ConfigureControls(config, applicationPath);
        ConfigureResources(config, applicationPath);

        config.DefaultCulture = "cs-CZ";
    }

    private void ConfigureRoutes(DotvvmConfiguration config, string applicationPath)
    {
        config.RouteTable.Add(RouteNames.Default, "", "Views/Default.dothtml");
        config.RouteTable.Add("Unauthorized", "odhlaseni", "Views/Unauthorized.dothtml");
        config.RouteTable.AutoDiscoverRoutes(new DefaultRouteStrategy(config));
    }

    private void ConfigureControls(DotvvmConfiguration config, string applicationPath)
    {
        // register code-only controls and markup controls
    }

    private void ConfigureResources(DotvvmConfiguration config, string applicationPath)
    {
        // register custom resources and adjust paths to the built-in resources
    }

    public void ConfigureServices(IDotvvmServiceCollection options)
    {
        options.AddDefaultTempStorages("temp");
        options.AddBusinessPack();
    }
}
