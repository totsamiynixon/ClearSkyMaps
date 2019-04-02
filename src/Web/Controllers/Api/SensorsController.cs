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
using Z.EntityFramework.Plus;

namespace ArduinoServer.Controllers.Api
{
    [RoutePrefix("api/sensors")]
    public class SensorsController : ApiController
    {
        private static IMapper _mapper = new Mapper(new MapperConfiguration(x =>
        {
            x.CreateMap<StaticSensor, StaticSensorModel>()
            .ForMember(f => f.PollutionLevel, m => m.ResolveUsing(p => PollutionHelper.GetPollutionLevel(p.Id)));
            x.CreateMap<Reading, StaticSensorReadingModel>();
        }));

        [HttpGet]
        public async Task<IHttpActionResult> GetAsync()
        {
            return Ok(_mapper.Map<List<StaticSensor>, List<StaticSensorModel>>(await DatabaseHelper.GetStaticSensorsAsync(true)));
        }

    }
}