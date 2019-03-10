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
            x.CreateMap<Sensor, SensorModel>()
            .ForMember(f => f.PollutionLevel, m => m.ResolveUsing(p => PollutionHelper.GetPollutionLevel(p.Id)));
            x.CreateMap<Reading, SensorReadingModel>();
        }));

        private readonly DataContext _context;
        public SensorsController()
        {
            _context = new DataContext();
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAsync()
        {
            return Ok(_mapper.Map<List<Sensor>, List<SensorModel>>(await DatabaseHelper.GetSensorsAsync()));
        }

        [HttpPost]
        public async Task<IHttpActionResult> RegisterAsync(RegisterSensorModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var sensor = await DatabaseHelper.AddSensorAsync(model.Latitude, model.Longitude);
            return Created("", sensor.TrackingKey);
        }
    }
}