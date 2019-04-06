using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Web.Helpers;
using Web.Models.Api.Notifications;

namespace Web.Areas.PWA.Controllers.Api
{
    [RoutePrefix("api/notifications")]
    public class NotificationsController : ApiController
    {
        private readonly string firebaseCloudMessagingServerKey;
        public NotificationsController()
        {
            firebaseCloudMessagingServerKey = SettingsHelper.FirebaseCloudMessagingServerKey;
        }

        [HttpPost]
        public async Task<IHttpActionResult> SubscribeOnSensorAsync(SubscribeOnSersorModel model)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("key", "=" + firebaseCloudMessagingServerKey);
                var request = new HttpRequestMessage(HttpMethod.Post, $"https://iid.googleapis.com/iid/v1/{model.RegistrationToken}/rel/topics/sensor_{model.SensorId}");
                request.Content = new StringContent("",
                                    Encoding.UTF8,
                                    "application/json");
                var response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        [HttpDelete]
        public async Task<IHttpActionResult> UnsubscribeFromSensorAsync(SubscribeOnSersorModel model)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("key", "=" + firebaseCloudMessagingServerKey);
                var request = new HttpRequestMessage(HttpMethod.Delete, $"https://iid.googleapis.com/iid/v1/{model.RegistrationToken}/rel/topics/sensor_{model.SensorId}");
                request.Content = new StringContent("",
                                    Encoding.UTF8,
                                    "application/json");
                var response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
        }
    }
}
