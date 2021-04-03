using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace FeatureFlags.Service
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    //Load the appsettings.json configuration file
                    config.AddUserSecrets<Program>(true);
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
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.CaptureStartupErrors(true);
                });
        }
    }
}
