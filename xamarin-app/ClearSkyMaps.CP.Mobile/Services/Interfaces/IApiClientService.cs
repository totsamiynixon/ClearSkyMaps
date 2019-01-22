using ClearSkyMaps.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClearSkyMaps.CP.Mobile.Interfaces
{
    public interface IApiClientService
    {
        Task<List<Sensor>> GetSensorsAsync();
    }
}
