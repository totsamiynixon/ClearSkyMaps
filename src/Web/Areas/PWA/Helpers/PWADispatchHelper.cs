using AutoMapper;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Areas.Admin.Hub;
using Web.Areas.Admin.Models.Hub;
using Web.Data.Models;
using Web.Enum;
using Web.Hub;
using Web.Models.Hub;

namespace Web.Areas.PWA.Helpers
{
    public static class PWADispatchHelper
    {
        private static readonly IMapper _mapper = new Mapper(new MapperConfiguration(x =>
        {
            x.CreateMap<Reading, ReadingDispatchModel>();
        }));

        public static void DispatchReadingsForStaticSensor(int sensorId, PollutionLevel pollutionLevel, Reading reading)
        {
            GlobalHost.ConnectionManager.GetHubContext<PWAStaticSensorHub, IPWAStaticSensorClient>().Clients.All.DispatchReading(new StaticSensorReadingDispatchModel
            {
                PollutionLevel = pollutionLevel,
                Reading = _mapper.Map<Reading, ReadingDispatchModel>(reading),
                SensorId = sensorId
            });
        }
    }
}