using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Web.Data;
using Web.Data.Models;
using Web.Models;
using Web.Models.Api.Sensor;
using Z.EntityFramework.Plus;

namespace ArduinoServer.Controllers.Api
{
    [RoutePrefix("api/sensors")]
    public class SensorsController : ApiController
    {
        private readonly DataContext _context;
        private static Random _random = new Random();
        public SensorsController()
        {
            _context = new DataContext();
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var sensors = await _context.Sensors.Select(f => new
            {
                Id = f.Id,
                Latitude = f.Latitude,
                Longitude = f.Longitude,
                Readings = f.Readings.OrderByDescending(z => z.Created).Take(10).Select(u => new
                {
                    CO2 = u.CO2,
                    LPG = u.LPG,
                    CO = u.CO,
                    CH4 = u.CH4,
                    Dust = u.Dust,
                    Temp = u.Temp,
                    Hum = u.Hum,
                    Preassure = u.Preassure,
                    Created = u.Created
                })
            }).ToListAsync();
            return Ok(sensors);
        }




        [HttpPost]
        public async Task<IHttpActionResult> Register(RegisterSensorModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var sensor = new Sensor
            {
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                TrackingKey = Guid.NewGuid().ToString()
            };
            _context.Sensors.Add(sensor);
            await _context.SaveChangesAsync();
            return Created("", sensor.TrackingKey);
        }
    }
}