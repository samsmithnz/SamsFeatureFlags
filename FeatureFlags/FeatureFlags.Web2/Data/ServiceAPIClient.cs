using FeatureFlags.Models;
using FeatureFlags.Web2.Data.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FeatureFlags.Web2.Data
{
    public class ServiceAPIClient : IServiceAPIClient
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;
        private string _degradedStateMessage = "The feature flags service is in a degraded state and cannot access data";

        public ServiceAPIClient(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new HttpClient
            {
                BaseAddress = new Uri(_configuration["AppSettings:WebServiceURL"])
            };
        }

        public async Task<Payload<List<FeatureFlag>>> GetFeatureFlags()
        {
            Uri url = new Uri($"api/FeatureFlags/GetFeatureFlags", UriKind.Relative);
            return await ReadMessageList<FeatureFlag>(url);
        }

        public async Task<Payload<FeatureFlag>> GetFeatureFlag(string name)
        {
            Uri url = new Uri($"api/FeatureFlags/GetFeatureFlag", UriKind.Relative);
            return await ReadMessageItem<FeatureFlag>(url);
        }

        public async Task<Payload<bool>> AddFeatureFlag(FeatureFlag featureFlag)
        {
            Uri url = new Uri($"api/FeatureFlags/SaveFeatureFlag", UriKind.Relative);
            return await PostMessageItem<bool>(url, featureFlag);
        }

        public async Task<Payload<bool>> DeleteFeatureFlag(FeatureFlag featureFlag)
        {
            Uri url = new Uri($"api/FeatureFlags/DeleteFeatureFlag", UriKind.Relative);
            return await PostMessageItem<bool>(url, featureFlag);
        }

        private async Task<Payload<List<T>>> ReadMessageList<T>(Uri url)
        {
            Payload<List<T>> data = new Payload<List<T>>();
            HttpResponseMessage response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode == true)
            {
                Stream stream = await response.Content.ReadAsStreamAsync();
                if (stream != null && stream.Length > 0)
                {
                    StreamReader reader = new(stream);
                    string text = reader.ReadToEnd();
                    data.Data = JsonConvert.DeserializeObject<List<T>>(text);
                }
                else
                {
                    data.ServiceMessage = _degradedStateMessage;
                    data.ServiceError = response.ToString();
                }
            }
            else
            {
                data.ServiceMessage = _degradedStateMessage;
                data.ServiceError = response.ToString();
            }
            return data;
        }

        private async Task<Payload<T>> ReadMessageItem<T>(Uri url)
        {
            Payload<T> data = new Payload<T>();
            HttpResponseMessage response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode == true)
            {
                Stream stream = await response.Content.ReadAsStreamAsync();
                if (stream != null && stream.Length > 0)
                {
                    StreamReader reader = new(stream);
                    string text = reader.ReadToEnd();
                    data.Data = JsonConvert.DeserializeObject<T>(text);
                }
                else
                {
                    data.ServiceMessage = _degradedStateMessage;
                    data.ServiceError = response.ToString();
                }
            }
            else
            {
                data.ServiceMessage = _degradedStateMessage;
                data.ServiceError = response.StatusCode + " " + response.ReasonPhrase;
            }
            return data;
        }

        private async Task<Payload<T>> PostMessageItem<T>(Uri url, FeatureFlag featureFlag)
        {
            Payload<T> data = new Payload<T>();
            string json = JsonConvert.SerializeObject(featureFlag, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(url, content);
            if (response.IsSuccessStatusCode == true)
            {
                Stream stream = await response.Content.ReadAsStreamAsync();
                if (stream != null && stream.Length > 0)
                {
                    StreamReader reader = new(stream);
                    string text = reader.ReadToEnd();
                    data.Data = JsonConvert.DeserializeObject<T>(text);
                }
                else
                {
                    data.ServiceMessage = _degradedStateMessage;
                    data.ServiceError = response.ToString();
                }
            }
            else
            {
                data.ServiceMessage = _degradedStateMessage;
                data.ServiceError = response.StatusCode + " " + response.ReasonPhrase;
            }
            return data;
        }

    }
}
