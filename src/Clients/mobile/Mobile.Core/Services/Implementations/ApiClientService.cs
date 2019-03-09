
using Mobile.Core.Config;
using Mobile.Core.Interfaces;
using Mobile.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.Core.Implementations
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
            return null;
        }
    }
}
