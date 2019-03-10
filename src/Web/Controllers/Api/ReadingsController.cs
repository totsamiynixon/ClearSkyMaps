using AutoMapper;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Web.Http;
using Web.Data.Models;
using Web.Helpers;
using Web.Hub;
using Web.Models.Api.Readings;
using Web.Models.Hub;

namespace ArduinoServer.Controllers.Api
{
    [RoutePrefix("api/readings")]
    public class ReadingsController : ApiController
    {
        private readonly IHubContext<IReadingsClient> _hubContext;

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
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostReading(SaveReadingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var sensor = await DatabaseHelper.GetSensorByTrackingKeyAsync(model.TrackingKey);
            if (sensor == null)
            {
                return NotFound();
            }
            var reading = _mapper.Map<SaveReadingModel, Reading>(model);
            reading.SensorId = sensor.Id;
            var result = DatabaseHelper.AddReadingAsync(reading);
            var hubReadings = new SensorReadingDispatchModel
            {
                SensorId = sensor.Id,
                PollutionLevel = PollutionHelper.GetPollutionLevel(sensor.Id),
                Reading = _mapper.Map<Reading, ReadingDispatchModel>(reading)
            };
            _hubContext.Clients.All.DispatchReading(hubReadings);
            return Ok();
        }
    }
}