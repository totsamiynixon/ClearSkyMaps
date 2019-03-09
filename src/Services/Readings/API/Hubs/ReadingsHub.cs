using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Readings.API.Models.Hub;

namespace Readings.API.Hubs
{
    public class ReadingsHub: Hub<IReadingsClient>
    {
        public async Task DispatchReadingAsync(SensorReadingDispatchModel reading)
        {
            await this.Clients.All.DispatchReadingAsync(reading);
        }
    }
}
