using DAL;
using Services.DTO.Sensor;
using Services.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Web.Models;

namespace ArduinoServer.Controllers.Api
{
    [RoutePrefix("api/sensors")]
    public class SensorsController : ApiController
    {
        private readonly ISensorService _sensorService;
        public SensorsController(ISensorService sensorService)
        {
            _sensorService = sensorService;
        }


        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await _sensorService.GetSensorListAsync());
        }


        [Route("checkTrackingKey")]
        [HttpGet]
        public async Task<IHttpActionResult> CheckTrackingKey([FromUri]string key)
        {
            return Ok(await _sensorService.CheckTrackingKeyAsync(key));
        }

        [HttpPost]
        public async Task<IHttpActionResult> Create(RegisterSensorDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var trackingKey = await _sensorService.RegisterAndGetTrackingKeyAsync(model);
            return Created("", trackingKey);
        }
    }
}