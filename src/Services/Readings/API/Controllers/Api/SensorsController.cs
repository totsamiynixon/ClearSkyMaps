using Microsoft.AspNetCore.Mvc;
using Readings.Services.DTO.Sensor;
using Readings.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Readings.API.Controllers.Api
{
    [Route("api/sensors")]
    [Produces("application/json")]
    public class SensorsController : Controller
    {
        private readonly ISensorService _sensorService;
        public SensorsController(ISensorService sensorService)
        {
            _sensorService = sensorService;
        }

        /// <summary>
        /// Returns full list of all sensor registered in system
        /// </summary>
        /// <response code="200">Returns list of all sensors with readings</response>
        /// <returns>List of sensors</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<SensorInfoDTO>), 200)]
        public async Task<IActionResult> Get()
        {
            return Ok(await _sensorService.GetSensorListAsync());
        }

        ///// <summary>
        ///// Is used for checkig is provided thacking key registered in system
        ///// </summary>
        ///// <param name="key">Tracking key</param>
        ///// <response code="200">Returns bool value</response>
        ///// <returns>true if sensor with such key is in system, otherwise, false</returns>
        //[Route("checkTrackingKey")]
        //[HttpGet]
        //[ProducesResponseType(typeof(bool), 200)]
        //public async Task<IActionResult> CheckTrackingKey(string key)
        //{
        //    return Ok(await _sensorService.CheckTrackingKeyAsync(key));
        //}
        /// <summary>
        /// Register sensor in system
        /// </summary>
        /// <param name="model">Model that represents sensor</param>      
        /// <response code="201">If sensor was successfuly registered in system</response>
        /// <response code="400">If sensor model is not valid</response>
        /// <returns>Tracking key</returns>
        [HttpPost]
        [ProducesResponseType(typeof(string), 201)]
        public async Task<IActionResult> Create(RegisterSensorDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var trackingKey = await _sensorService.RegisterAndGetTrackingKeyAsync(model);
            return Created("api/sensors", trackingKey);
        }
    }
}