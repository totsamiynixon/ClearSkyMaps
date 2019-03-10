using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Web.Data;
using Web.Data.Models;
using Web.Models.Api.Subscribers;

namespace Web.Controllers.Api
{
    [RoutePrefix("api/subscribers")]
    public class SubscribersController : ApiController
    {
        private readonly DataContext _context;

        public SubscribersController()
        {
            _context = new DataContext();
        }

        [HttpPost]
        public async Task<IHttpActionResult> CreateSubscription(CreateSubscriptionModel model)
        {
            var subscriber = new Subscriber
            {
                Sensors = await _context.Sensors.Where(f => model.SensorIds.Any(z => z == f.Id)).ToListAsync()
            };
            _context.Subscribers.Add(subscriber);
            await _context.SaveChangesAsync();
            return Created("api/subscribers", subscriber.Id);
        }


        [HttpPut]
        public async Task<IHttpActionResult> UpdateSubscriptionSubscription(UpdateSubscriptionModel model)
        {
            var subscriber = await _context.Subscribers.FirstOrDefaultAsync(f => f.Id == model.Id);
            var sensors = await _context.Sensors.Where(f => model.SensorIds.Any(z => z == f.Id)).ToListAsync();
            subscriber.Sensors = sensors;
            _context.Entry(subscriber).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
