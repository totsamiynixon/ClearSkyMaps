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

namespace Web.Helpers
{
    public static class DispatchHelper
    {
        private static readonly IMapper _mapper = new Mapper(new MapperConfiguration(x =>
        {
            x.CreateMap<Reading, StaticSensorReadingDispatchModel>();
        }));

        public static void DispatchReadingsForStaticSensor(int sensorId, PollutionLevel pollutionLevel, Reading reading)
        {
            GlobalHost.ConnectionManager.GetHubContext<StaticSensorHub, IStaticSensorClient>().Clients.All.DispatchReading(_mapper.Map<Reading, StaticSensorReadingDispatchModel>(reading));
        }

        public static void DispatchReadingsForPortableSensor(int sensorId, Reading reading)
        {
            GlobalHost.ConnectionManager.GetHubContext<PortableSensorHub, IPortableSensorClient>().Clients.All.DispatchReading(_mapper.Map<Reading, PortableSensorReadingsDispatchModel>(reading));
        }

        public static void DispatchCoordinatesForPortableSensor(int sensorId, double latitude, double longitude)
        {
            GlobalHost.ConnectionManager.GetHubContext<PortableSensorHub, IPortableSensorClient>().Clients.All.DispatchCoordinates(new PortableSensorCoordinatesDispatchModel {
                Latitude = latitude,
                Longitude = longitude
            });
        }
    }
}