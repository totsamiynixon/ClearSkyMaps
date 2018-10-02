using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Services.DTO.Reading;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Web.Core.Hubs;
using Web.Core.Models.Hub;

namespace Web.Core.Controllers.Api
{
    [Route("api/sensors")]
    public class ReadingsController : Controller
    {
        private readonly IHubContext<ReadingsHub, IReadingsClient> _hubContext;
        private readonly IReadingService _readingService;
        private readonly ISensorService _sensorService;
        public ReadingsController(IReadingService readingService, ISensorService sensorService, IHubContext<ReadingsHub, IReadingsClient> hubContext)
        {
            _hubContext = hubContext;
            _readingService = readingService;
            _sensorService = sensorService;
        }

        [HttpPost]
        public async Task<IActionResult> PostReading(string trackingKey, SaveReadingDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _readingService.SaveReadingAsync(trackingKey, model);
            var sensorPollutionModel = await _sensorService.GetSensorPollutionLevelInfoAsync(trackingKey);
            var hubReadings = new SensorReadingDispatchModel
            {
                SensorId = sensorPollutionModel.sensorId,
                LatestPollutionLevel = sensorPollutionModel.level,
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
                    Created = model.Created
                }
            };
            await _hubContext.Clients.All.DispatchReadingAsync(hubReadings);
            return Ok();
        }
    }
}