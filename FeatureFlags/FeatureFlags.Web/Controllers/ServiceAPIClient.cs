using FeatureFlags.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FeatureFlags.Web.Controllers
{
    public class ServiceAPIClient : IServiceAPIClient
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;

        public ServiceAPIClient(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new HttpClient
            {
                BaseAddress = new Uri(_configuration["AppSettings:WebServiceURL"])
            };
        }

        public async Task<List<FeatureFlag>> GetFeatureFlags()
        {
            Uri url = new Uri($"api/FeatureFlags/GetFeatureFlags", UriKind.Relative);
            return await ReadMessageList<FeatureFlag>(url);
        }

        public async Task<FeatureFlag> GetFeatureFlag(string name)
        {
            Uri url = new Uri($"api/FeatureFlags/GetFeatureFlag", UriKind.Relative);
            return await ReadMessageItem<FeatureFlag>(url);
        }

        public async Task<bool> AddFeatureFlag(FeatureFlag featureFlag)
        {
            Uri url = new Uri($"api/FeatureFlags/SaveFeatureFlag", UriKind.Relative);
            return await PostMessageItem<bool>(url, featureFlag);
        }

        public async Task<bool> DeleteFeatureFlag(FeatureFlag featureFlag)
        {
            Uri url = new Uri($"api/FeatureFlags/DeleteFeatureFlag", UriKind.Relative);
            return await PostMessageItem<bool>(url, featureFlag);
        }

        private async Task<List<T>> ReadMessageList<T>(Uri url)
        {
            HttpResponseMessage response = await _client.GetAsync(url);
            return await response.Content.ReadAsAsync<List<T>>();
        }

        private async Task<T> ReadMessageItem<T>(Uri url)
        {
            HttpResponseMessage response = await _client.GetAsync(url);
            return await response.Content.ReadAsAsync<T>();
        }

        private async Task<T> PostMessageItem<T>(Uri url, FeatureFlag featureFlag)
        {
            string json = JsonConvert.SerializeObject(featureFlag, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(url, content);
            return await response.Content.ReadAsAsync<T>();
        }

    }
}
