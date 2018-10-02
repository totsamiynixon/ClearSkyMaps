using Microsoft.AspNetCore.Mvc;
using Services.DTO.Sensor;
using Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Core.Controllers.Api
{
    [Route("api/sensors")]
    public class SensorsController : Controller
    {
        private readonly ISensorService _sensorService;
        public SensorsController(ISensorService sensorService)
        {
            _sensorService = sensorService;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _sensorService.GetSensorListAsync());
        }


        [Route("checkTrackingKey")]
        [HttpGet]
        public async Task<IActionResult> CheckTrackingKey(string key)
        {
            return Ok(await _sensorService.CheckTrackingKeyAsync(key));
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterSensorDTO model)
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