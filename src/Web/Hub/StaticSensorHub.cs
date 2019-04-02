using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Microsoft.AspNet.SignalR;
using Web.Data.Models;
using Web.Models.Hub;

namespace Web.Hub
{
    public class StaticSensorHub : Microsoft.AspNet.SignalR.Hub<IStaticSensorClient>
    {
        private static readonly IMapper _mapper = new Mapper(new MapperConfiguration(x =>
        {
            x.CreateMap<Reading, StaticSensorReadingDispatchModel>();
        }));

        public void DispatchReadings(Reading reading)
        {
            Clients.All.DispatchReading(_mapper.Map<Reading, StaticSensorReadingDispatchModel>(reading));
        }
    }
}