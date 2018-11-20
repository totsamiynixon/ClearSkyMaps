using ClearSkyMaps.Xamarin.Forms.Config;
using ClearSkyMaps.Xamarin.Forms.Models;
using ClearSkyMaps.Xamarin.Forms.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ClearSkyMaps.Xamarin.Forms.Services.Implementations
{
    public class ApiClientService : IApiClientService
    {
        private readonly AppConfig _config;
        private readonly HttpClient _baseClient;
        public ApiClientService(AppConfig config)
        {
            _config = config;
            _baseClient = new HttpClient();
            _baseClient.BaseAddress = new Uri($"{_config.BaseServiceUrl}/api/");
        }
        public async Task<List<Sensor>> GetSensorsAsync()
        {
            var response = await _baseClient.GetAsync("sensors");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Sensor>>(content);
            }
            else
            {
                throw new Exception($"Can't get data from server! Reason: {response.ReasonPhrase}");
            }
        }
    }
}
