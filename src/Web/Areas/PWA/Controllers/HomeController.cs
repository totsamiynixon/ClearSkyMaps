using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.UI;
using Web.Helpers;
using FileIO = System.IO.File;

namespace Web.Areas.PWA.Controllers
{
    [RouteArea("PWA")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(Duration = 3600, Location = OutputCacheLocation.Server)]
        public async Task<ActionResult> ManifestJsonAsync()
        {
            using (var reader = FileIO.OpenText(HostingEnvironment.MapPath(@"~/Areas/PWA/manifest.json")))
            {
                var jsonString = await reader.ReadToEndAsync();
                var manifest = JsonConvert.DeserializeObject<JObject>(jsonString);
                if (!SettingsHelper.IsProduction)
                {
                    manifest["name"] = manifest["name"] + " " + SettingsHelper.ApplicationEnvironment;
                    manifest["short_name"] = manifest["short_name"] + " " + SettingsHelper.ApplicationEnvironment;
                }
                manifest["gsm_sender_id"] = SettingsHelper.FirebaseCloudMessagingMessagingSenderId;
                if (SettingsHelper.IsDevelopment)
                {
                    Response.Cache.SetNoStore();
                    Response.Cache.SetNoServerCaching();
                }
                return Content(JsonConvert.SerializeObject(manifest), "application/json");
            }
        }

        [OutputCache(Duration = 3600, Location = OutputCacheLocation.Server)]
        public async Task<ActionResult> ServiceWorkerJsAsync()
        {
            using (var reader = FileIO.OpenText(HostingEnvironment.MapPath(@"~/Areas/PWA/service-worker.js")))
            {
                var jsString = await reader.ReadToEndAsync();
                jsString = jsString.Replace("%MessagingSenderId%", SettingsHelper.FirebaseCloudMessagingMessagingSenderId);
                jsString = jsString.Replace("%Version%", SettingsHelper.ApplicationVersion);
                if (SettingsHelper.IsDevelopment)
                {
                    Response.Cache.SetNoStore();
                    Response.Cache.SetNoServerCaching();
                }
                return Content(jsString, "text/javascript");
            }
        }
    }
}
