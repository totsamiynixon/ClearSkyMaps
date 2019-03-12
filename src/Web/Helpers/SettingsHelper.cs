using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace Web.Helpers
{
    public static class SettingsHelper
    {

        private static JObject Settings { get; set; }

        private static readonly List<string> requiredSettings = new List<string>()
        {
            "FirebaseCloudMessaging:ServerKey",
            "FirebaseCloudMessaging:MessagingSenderId",
            "Application:Version",
            "Application:Environment",
            "ConnectionString",
            "Emulation:Enabled"
        };

        static SettingsHelper()
        {
            using (StreamReader streamReader = new StreamReader(HostingEnvironment.MapPath(@"~/config.json"), Encoding.UTF8))
            {
                var jsonString = streamReader.ReadToEnd();
                Settings = JsonConvert.DeserializeObject<JObject>(jsonString);
                if (IsDevelopment)
                {
                    Settings["Application:Version"] = $"{DateTime.UtcNow.Day}.{DateTime.UtcNow.Millisecond}";
                }
                var notprovidedKeys = requiredSettings.Except(requiredSettings.Intersect(Settings.Properties().Select(f => f.Name))).ToList();
                if (notprovidedKeys.Any())
                {
                    throw new KeyNotFoundException($"No such keys:[{string.Join(", ", notprovidedKeys)}] in config.json");
                }
            }
        }


        public static bool IsDevelopment => ApplicationEnvironment == "Development";

        public static bool IsStaging => ApplicationEnvironment == "Staging";

        public static bool IsProduction => ApplicationEnvironment == "Production";

        public static bool EmulationEnabled => bool.Parse(Settings["Emulation:Enabled"].ToString());

        public static string FirebaseCloudMessagingServerKey => Settings["FirebaseCloudMessaging:ServerKey"].ToString();

        public static string FirebaseCloudMessagingMessagingSenderId => Settings["FirebaseCloudMessaging:MessagingSenderId"].ToString();

        public static string ApplicationVersion => Settings["Application:Version"].ToString();

        public static string ApplicationEnvironment => Settings["Application:Environment"].ToString();

        public static string ConnectionString => Settings["ConnectionString"].ToString();
    }
}