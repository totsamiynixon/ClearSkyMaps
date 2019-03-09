using Mobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.Core.Interfaces
{
    public interface IApiClientService
    {
        Task<List<Sensor>> GetSensorsAsync();
    }
}
