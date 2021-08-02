using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FeatureFlagsDemo.Web.Controllers
{
    public class FeatureFlagsServiceApiClient : IFeatureFlagsServiceApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;

        public FeatureFlagsServiceApiClient(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new HttpClient
            {
                BaseAddress = new Uri(_configuration["AppSettings:FeatureFlagsWebServiceURL"])
            };
        }

        public async Task<bool> CheckFeatureFlag(string name, string environment)
        {
            if (environment.ToLower().StartsWith("dev") == true)
            {
                environment = "dev";
            }
            else if (environment.ToLower().StartsWith("qa") == true || environment.ToLower().StartsWith("test") == true) //sometimes we have a test environment that we want to use qa settings
            {
                environment = "qa";
            }
            else if (environment.ToLower().StartsWith("prod") == true)
            {
                environment = "prod";
            }
            else if (environment.ToLower().StartsWith("pr") == true) //last so that it doesn't trigger with prod. :P
            {
                environment = "pr";
            }
            Uri url = new($"api/FeatureFlags/CheckFeatureFlag?name=" + name + "&environment=" + environment, UriKind.Relative);
            return await ReadMessageItem(url);
        }

        private async Task<bool> ReadMessageItem(Uri url)
        {
            HttpResponseMessage response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode == false)
            {
                return (bool)false;
            }
            else
            {
                string bodyContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<bool>(bodyContent);
            }
        }
    }
}
