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
using Web.Models.Hub;

namespace Web.Areas.Admin.Helpers
{
    public static class AdminDispatchHelper
    {
        private static readonly IMapper _mapper = new Mapper(new MapperConfiguration(x =>
        {
            x.CreateMap<Reading, StaticSensorReadingDispatchModel>();
        }));

        public static void DispatchReadingsForStaticSensor(int sensorId, Reading reading)
        {
            GlobalHost.ConnectionManager.GetHubContext<AdminStaticSensorHub, IAdminStaticSensorClient>().Clients.All.DispatchReading(_mapper.Map<Reading, StaticSensorReadingDispatchModel>(reading));
        }

        public static void DispatchReadingsForPortableSensor(int sensorId, Reading reading)
        {
            GlobalHost.ConnectionManager.GetHubContext<AdminPortableSensorHub, IAdminPortableSensorClient>().Clients.Group(AdminPortableSensorHub.PortableSensorGroup(sensorId)).DispatchReading(_mapper.Map<Reading, PortableSensorReadingsDispatchModel>(reading));
        }

        public static void DispatchCoordinatesForPortableSensor(int sensorId, double latitude, double longitude)
        {
            GlobalHost.ConnectionManager.GetHubContext<AdminPortableSensorHub, IAdminPortableSensorClient>().Clients.Group(AdminPortableSensorHub.PortableSensorGroup(sensorId)).DispatchCoordinates(new PortableSensorCoordinatesDispatchModel
            {
                Latitude = latitude,
                Longitude = longitude
            });
        }
    }
}