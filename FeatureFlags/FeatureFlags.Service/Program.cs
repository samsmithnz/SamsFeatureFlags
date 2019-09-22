using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Logging;

namespace FeatureFlags.Service
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseApplicationInsights()
                .ConfigureAppConfiguration((context, config) =>
                {
                    //Load the appsettings.json configuration file
                    IConfigurationRoot buildConfig = config.Build();

                    ////Load a connection to our Azure key vault instance
                    string azureKeyVaultURL = buildConfig["AppSettings:KeyVaultURL"];
                    string clientId = buildConfig["AppSettings:ClientId"];
                    string clientSecret = buildConfig["AppSettings:ClientSecret"];
                    //AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
                    //KeyVaultClient keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
                    //config.AddAzureKeyVault(buildConfig["AppSettings:KeyVaultURL"], keyVaultClient, new DefaultKeyVaultSecretManager());
                    config.AddAzureKeyVault(azureKeyVaultURL, clientId, clientSecret);

                })
                .UseStartup<Startup>();
    }
}
