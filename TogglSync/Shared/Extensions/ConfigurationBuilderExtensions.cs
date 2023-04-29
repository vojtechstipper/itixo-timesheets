using Azure.Identity;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using System;

namespace TogglSyncShared.Extensions;

public static class ConfigurationBuilderExtensions
{
    public static void AddAzureKeyVault(this IConfigurationBuilder configurationBuilder)
    {
        var azureServiceTokenProvider = new AzureServiceTokenProvider();
        var keyVaultClient = new KeyVaultClient(
            new KeyVaultClient.AuthenticationCallback(
                azureServiceTokenProvider.KeyVaultTokenCallback));

        IConfigurationRoot configuration = configurationBuilder.Build();
        configurationBuilder.AddConfiguration(configuration)
        .AddAzureKeyVault(new Uri(configuration.GetConnectionString("VaultUri")), new DefaultAzureCredential());
    }
}
