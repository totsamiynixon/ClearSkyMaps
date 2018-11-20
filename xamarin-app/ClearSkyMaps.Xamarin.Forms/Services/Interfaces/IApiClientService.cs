using ClearSkyMaps.Xamarin.Forms.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClearSkyMaps.Xamarin.Forms.Services.Interfaces
{
    public interface IApiClientService
    {
        Task<List<Sensor>> GetSensorsAsync();
    }
}
