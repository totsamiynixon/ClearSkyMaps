using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Web.Data;
using Web.Data.Models;
using Web.Enum;
using Web.Helpers;
using Web.Models;
using Web.Models.Api.Sensor;
using Web.Models.Cache;
using Z.EntityFramework.Plus;

namespace Web.Areas.PWA.Controllers
{
    [RoutePrefix("api/sensors")]
    public class SensorsController : ApiController
    {
        private static IMapper _mapper = new Mapper(new MapperConfiguration(x =>
        {
            x.CreateMap<StaticSensor, StaticSensorModel>();
            x.CreateMap<Reading, StaticSensorReadingModel>();
        }));

        [HttpGet]
        public async Task<IHttpActionResult> GetAsync()
        {
            var sensors = await SensorCacheHelper.GetStaticSensorsAsync();
            var model = sensors.Select(f => new StaticSensorModel
            {
                Id = f.Sensor.Id,
                Latitude = f.Sensor.Latitude,
                Longitude = f.Sensor.Longitude,
                PollutionLevel = f.PollutionLevel,
                Readings = _mapper.Map<List<Reading>, List<StaticSensorReadingModel>>(f.Sensor.Readings)
            });
            return Ok(model.ToArray());
        }

    }
}