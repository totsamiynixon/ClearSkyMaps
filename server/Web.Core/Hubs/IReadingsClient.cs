using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Core.Models.Hub;

namespace Web.Core.Hubs
{
    public interface IReadingsClient
    {
        Task DispatchReadingAsync(SensorReadingDispatchModel reading);
    }
}
