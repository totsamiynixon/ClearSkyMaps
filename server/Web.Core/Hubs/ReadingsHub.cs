using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Web.Core.Models.Hub;

namespace Web.Core.Hubs
{
    public class ReadingsHub: Hub<IReadingsClient>
    {
        public async Task DispatchReadingAsync(SensorReadingDispatchModel reading)
        {
            await this.Clients.All.DispatchReadingAsync(reading);
        }
    }
}
