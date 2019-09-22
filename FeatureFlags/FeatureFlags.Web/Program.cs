using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FeatureFlags.Web
{
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
                    //AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
                    //KeyVaultClient keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
                    //config.AddAzureKeyVault(buildConfig["AppSettings:KeyVaultURL"], keyVaultClient, new DefaultKeyVaultSecretManager());

                })
                .UseStartup<Startup>();
    }
}
