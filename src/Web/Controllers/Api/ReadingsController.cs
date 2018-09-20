using AutoMapper;
using DAL;
using Microsoft.AspNet.SignalR;
using Services.DTO.Reading;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Web.Hub;
using Web.Models;
using Web.Models.Hub;

namespace ArduinoServer.Controllers.Api
{
    [RoutePrefix("api/sensors")]
    public class ReadingsController : ApiController
    {
        private readonly IHubContext<IReadingsClient> _hubContext;
        private readonly IReadingService _readingService;
        private readonly ISensorService _sensorService;
        public ReadingsController(IReadingService readingService, ISensorService sensorService)
        {
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<ReadingsHub, IReadingsClient>();
            _readingService = readingService;
            _sensorService = sensorService;
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostReading(string trackingKey, SaveReadingDTO model)
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
            _hubContext.Clients.All.DispatchReading(hubReadings);
            return Ok();
        }
    }
}