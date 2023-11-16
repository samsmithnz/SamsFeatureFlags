using FeatureFlags.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FeatureFlags.Web.Controllers
{
    public class ServiceAPIClient : IServiceAPIClient
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;
        //private string _degradedStateMessage = "The feature flags service is in a degraded state and cannot access data";

        public ServiceAPIClient(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new HttpClient
            {
                BaseAddress = new(_configuration["AppSettings:WebServiceURL"])
            };
        }

        public async Task<List<FeatureFlag>> GetFeatureFlags()
        {
            Uri url = new($"api/FeatureFlags/GetFeatureFlags", UriKind.Relative);
            return await ReadMessageList<FeatureFlag>(url);
        }

        public async Task<FeatureFlag> GetFeatureFlag(string name)
        {
            Uri url = new($"api/FeatureFlags/GetFeatureFlag", UriKind.Relative);
            return await ReadMessageItem<FeatureFlag>(url);
        }

        public async Task<bool> AddFeatureFlag(FeatureFlag featureFlag)
        {
            Uri url = new($"api/FeatureFlags/SaveFeatureFlag", UriKind.Relative);
            return await PostMessageItem<bool>(url, featureFlag);
        }

        public async Task<bool> DeleteFeatureFlag(FeatureFlag featureFlag)
        {
            Uri url = new Uri($"api/FeatureFlags/DeleteFeatureFlag", UriKind.Relative);
            return await PostMessageItem<bool>(url, featureFlag);
        }

        private async Task<List<T>> ReadMessageList<T>(Uri url)
        {
            List<T> data = new();
            HttpResponseMessage response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode == true)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                data = JsonConvert.DeserializeObject<List<T>>(responseString);
            }
            return data;
        }

        private async Task<T?> ReadMessageItem<T>(Uri url)
        {
            T? data = default;
            HttpResponseMessage response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode == true)
            {
                data = await response.Content.ReadAsAsync<T>();
            }
            return data;
        }

        private async Task<T?> PostMessageItem<T>(Uri url, FeatureFlag featureFlag)
        {
            T? data = default;
            string json = JsonConvert.SerializeObject(featureFlag, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            StringContent content = new(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(url, content);
            if (response.IsSuccessStatusCode == true)
            {
                data = await response.Content.ReadAsAsync<T>();
            }

            return data;
        }

    }
}
