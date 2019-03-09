using AutoMapper;
using Microsoft.AspNet.SignalR;
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
using Web.Data;
using Web.Data.Models;
using Web.Enum;
using Web.Hub;
using Web.Models;
using Web.Models.Api.Readings;
using Web.Models.Hub;

namespace ArduinoServer.Controllers.Api
{
    [RoutePrefix("api/readings")]
    public class ReadingsController : ApiController
    {
        private readonly IHubContext<IReadingsClient> _hubContext;
        private readonly DataContext _context;
        private static Random _random = new Random();

        private static IMapper _mapper = new Mapper(new MapperConfiguration(
            x =>
                {
                    x.CreateMap<SaveReadingModel, Reading>();
                    x.CreateMap<Reading, ReadingDispatchModel>();
                }
        ));

        public ReadingsController()
        {
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<ReadingsHub, IReadingsClient>();
            _context = new DataContext();
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostReading(SaveReadingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var sensor = await _context.Sensors.AsNoTracking().FirstOrDefaultAsync(f => f.TrackingKey == model.TrackingKey);
            if (sensor == null)
            {
                return NotFound();
            }
            var reading = _mapper.Map<SaveReadingModel, Reading>(model);
            reading.SensorId = sensor.Id;
            _context.Readings.Add(reading);
            await _context.SaveChangesAsync();
            var hubReadings = new SensorReadingDispatchModel
            {
                SensorId = sensor.Id,
                PollutionLevel = (PollutionLevel)_random.Next(0, 2),
                Reading = _mapper.Map<Reading, ReadingDispatchModel>(reading)
            };
            _hubContext.Clients.All.DispatchReading(hubReadings);
            return Ok();
        }
    }
}