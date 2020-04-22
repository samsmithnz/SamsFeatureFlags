using FeatureFlags.Models;
using FeatureFlags.Web.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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

        public async Task<Data<List<FeatureFlag>>> GetFeatureFlags()
        {
            Uri url = new Uri($"api/FeatureFlags/GetFeatureFlags", UriKind.Relative);
            return await ReadMessageList<FeatureFlag>(url);
        }

        public async Task<Data<FeatureFlag>> GetFeatureFlag(string name)
        {
            Uri url = new Uri($"api/FeatureFlags/GetFeatureFlag", UriKind.Relative);
            return await ReadMessageItem<FeatureFlag>(url);
        }

        public async Task<Data<bool>> AddFeatureFlag(FeatureFlag featureFlag)
        {
            Uri url = new Uri($"api/FeatureFlags/SaveFeatureFlag", UriKind.Relative);
            return await PostMessageItem<bool>(url, featureFlag);
        }

        public async Task<Data<bool>> DeleteFeatureFlag(FeatureFlag featureFlag)
        {
            Uri url = new Uri($"api/FeatureFlags/DeleteFeatureFlag", UriKind.Relative);
            return await PostMessageItem<bool>(url, featureFlag);
        }

        private async Task<Data<List<T>>> ReadMessageList<T>(Uri url)
        {
            Data<List<T>> data = new Data<List<T>>();
            HttpResponseMessage response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode == true)
            {
                data.Payload = await response.Content.ReadAsAsync<List<T>>();
            }
            else
            {
                data.Message = "The feature flags service is in a degraded state and cannot be accessed";
                data.Error = response.ToString();
            }
            return data;
        }

        private async Task<Data<T>> ReadMessageItem<T>(Uri url)
        {
            Data<T> data = new Data<T>();
            HttpResponseMessage response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode == true)
            {
                data.Payload = await response.Content.ReadAsAsync<T>();
            }
            else
            {
                data.Message = "The feature flags service is in a degraded state and cannot be accessed";
                data.Error = response.StatusCode + " " + response.ReasonPhrase;
            }
            return data;
        }

        private async Task<Data<T>> PostMessageItem<T>(Uri url, FeatureFlag featureFlag)
        {
            Data<T> data = new Data<T>();
            string json = JsonConvert.SerializeObject(featureFlag, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(url, content);
            if (response.IsSuccessStatusCode == true)
            {
                data.Payload = await response.Content.ReadAsAsync<T>();
            }
            else
            {
                data.Message = "The feature flags service is in a degraded state and cannot be accessed";
                data.Error = response.StatusCode + " " + response.ReasonPhrase;
            }
            return data;
        }

    }
}
