using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Web.Helpers
{
    public static class PWAHelper
    {
        public static void InitPWA()
        {
            InitManifestJson();
            InitServiceWorkerJs();
        }

        private static void InitManifestJson()
        {
            var path = HostingEnvironment.MapPath(@"~/manifest.json");
            var jsonString = File.ReadAllText(path);
            var manifest = JsonConvert.DeserializeObject<JObject>(jsonString);
            if (!SettingsHelper.IsProduction)
            {
                manifest["name"] = manifest["name"] + " " + SettingsHelper.ApplicationEnvironment;
                manifest["short_name"] = manifest["short_name"] + " " + SettingsHelper.ApplicationEnvironment;
            }
            manifest["gsm_sender_id"] = SettingsHelper.FirebaseCloudMessagingMessagingSenderId;
            File.WriteAllText(path, JsonConvert.SerializeObject(manifest));
        }

        private static void InitServiceWorkerJs()
        {
            var path = HostingEnvironment.MapPath(@"~/Scripts/front/service-worker.js");
            var jsonString = File.ReadAllText(path);
            var manifest = JsonConvert.DeserializeObject<JObject>(jsonString);
            if (!SettingsHelper.IsProduction)
            {
                manifest["name"] = manifest["name"] + " " + SettingsHelper.ApplicationEnvironment;
                manifest["short_name"] = manifest["short_name"] + " " + SettingsHelper.ApplicationEnvironment;
            }
            manifest["gsm_sender_id"] = SettingsHelper.FirebaseCloudMessagingMessagingSenderId;
            File.WriteAllText(path, JsonConvert.SerializeObject(manifest));
        }
    }
}