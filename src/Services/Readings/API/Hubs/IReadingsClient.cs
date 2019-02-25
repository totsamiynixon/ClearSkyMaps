using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Readings.API.Models.Hub;

namespace Readings.API.Hubs
{
    public interface IReadingsClient
    {
        Task DispatchReadingAsync(SensorReadingDispatchModel reading);
    }
}
