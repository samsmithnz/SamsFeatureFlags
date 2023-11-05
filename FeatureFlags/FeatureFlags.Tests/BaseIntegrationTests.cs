using Azure.Core;
using Azure.Identity;
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
            config.AddUserSecrets<BaseIntegrationTests>(true);
            Configuration = config.Build();

            //Load a connection to our Azure key vault instance
            string? azureKeyVaultURL = Configuration["AppSettings:KeyVaultURL"];
            string? clientId = Configuration["AppSettings:ClientId"];
            string? clientSecret = Configuration["AppSettings:ClientSecret"];
            string? tenantId = Configuration["AppSettings:TenantId"];

            if (azureKeyVaultURL != null && clientId != null && clientSecret != null && tenantId != null)
            {
                TokenCredential tokenCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
                config.AddAzureKeyVault(new(azureKeyVaultURL), tokenCredential);
            }
            else
            {
                throw new System.Exception("Missing configuration for Azure Key Vault");
            }
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