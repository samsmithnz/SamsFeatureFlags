using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;

namespace FeatureFlags.Tests
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class BaseIntegrationTests
    {
        private TestServer? _server;
        public HttpClient? Client;
        public IConfigurationRoot? Configuration;
        // public IDatabase Database;

        [TestInitialize]
        public void TestStartUp()
        {
            IConfigurationBuilder config = new ConfigurationBuilder()
               .SetBasePath(AppContext.BaseDirectory)
               .AddJsonFile("appsettings.json");
            config.AddUserSecrets<BaseIntegrationTests>();
            Configuration = config.Build();

            string azureKeyVaultURL = Configuration["AppSettings:KeyVaultURL"];
            string clientId = Configuration["AppSettings:ClientId"];
            string clientSecret = Configuration["AppSettings:ClientSecret"];
            //AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
            //KeyVaultClient keyVaultClient = new KeyVaultClient(
            //    new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
            //config.AddAzureKeyVault(azureKeyVaultURL, keyVaultClient, new DefaultKeyVaultSecretManager());
            config.AddAzureKeyVault(azureKeyVaultURL, clientId, clientSecret);
            Configuration = config.Build();

            //ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(Configuration["RedisCacheConnectionString:CacheConnection"]);
            //Database = connectionMultiplexer.GetDatabase(0); //TODO: do we need the 0?

            //Setup the test server
            _server = new TestServer(WebHost.CreateDefaultBuilder()
                .UseConfiguration(Configuration)
                .UseStartup<FeatureFlags.Service.Startup>());
            Client = _server.CreateClient();
            //Client.BaseAddress = new Uri(Configuration["AppSettings:WebServiceURL"]);
        }
    }
}