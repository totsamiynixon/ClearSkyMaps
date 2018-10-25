using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Services.DTO.Models.Reading;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Web.Core.Hubs;
using Web.Core.Models.Api.Readings;
using Web.Core.Models.Hub;

namespace Web.Core.Controllers.Api
{
    [Route("api/readings")]
    [Produces("application/json")]
    public class ReadingsController : Controller
    {
        private readonly IHubContext<ReadingsHub, IReadingsClient> _hubContext;
        private readonly IReadingService _readingService;
        private readonly ISensorService _sensorService;
        private readonly IMapper _mapper;
        public ReadingsController(
            IReadingService readingService,
            ISensorService sensorService,
            IHubContext<ReadingsHub, IReadingsClient> hubContext,
            Func<Type, IMapper> serviceAccessor)
        {
            _hubContext = hubContext;
            _readingService = readingService;
            _sensorService = sensorService;
            _mapper = serviceAccessor(typeof(ReadingsController));
        }
        /// <summary>
        /// Saves reading into system for sensor with specified tracking key   
        /// </summary>
        /// <param name="model">The data model</param>
        /// <response code="200">Readings was successfuly posted</response>
        /// <response code="400">Model is not valid</response>
        /// <response code="404">Sensor with such tracking key was not found</response>  
        /// <returns>Status code</returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PostReading(ApiPostReadingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            SaveReadingResultDTO result = null;
            try
            {
                result = await _readingService.SaveReadingAsync(model.SensorTrackingKey, _mapper.Map<ApiPostReadingModel, SaveReadingDTO>(model));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            var sensorPollutionLevel = await _sensorService.GetSensorPollutionLevelInfoAsync(model.SensorTrackingKey);
            var hubReadings = new SensorReadingDispatchModel
            {
                SensorId = result.SensorId,
                LatestPollutionLevel = sensorPollutionLevel,
                Reading = new SensorReadingDTO
                {
                    CH4 = model.CH4,
                    CO = model.CO,
                    CO2 = model.CO2,
                    Dust = model.Dust,
                    Hum = model.Hum,
                    LPG = model.LPG,
                    Preassure = model.Preassure,
                    Temp = model.Temp,
                    Created = result.Created
                }
            };
            await _hubContext.Clients.All.DispatchReadingAsync(hubReadings);
            return Ok();
        }
    }
}