using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using System.Xml;

namespace Web.Config
{
    public class MultiStageConfigBuilder : ConfigurationBuilder
    {
        private readonly IDictionary Settings;

        private static readonly List<string> requiredDevSettings = new List<string>()
        {
            "FirebaseCloudMessaging:ServerKey",
            "FirebaseCloudMessaging:MessagingSenderId",
            "Application:Version",
            "Application:Environment",
            "DefaultConnenction",
            "Emulation:Enabled"
        };

        private static readonly Dictionary<string, string> stagingProdSettingsMap = new Dictionary<string, string>()
        {
            {"FIREBASE_CLOUD_MESSAGING__SERVER_KEY", "FirebaseCloudMessaging:ServerKey"},
            {"FIREBASE_CLOUD_MESSAGING__MESSAGING_SENDER_ID", "FirebaseCloudMessaging:MessagingSenderId"},
            {"APPLICATION_ENVIRONMENT", "Application:Version" },
            {"APPLICATION_VERSION", "Application:Environment" },
            {"CONNECTION_STRING","DefaultConnenction"},
            {"EMULATION_ENABLED","Emulation:Enabled"}
        };

        public MultiStageConfigBuilder()
        {
            var appEnv = Environment.GetEnvironmentVariable("APPLICATION_ENVIRONMENT");
            if (string.IsNullOrEmpty(appEnv))
            {
                var path = HostingEnvironment.MapPath(@"~/dev-config.json");
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException("dev-config.json at the root of the project was not found!");
                }
                string jsonConfig = File.ReadAllText(path);
                Settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonConfig);
                Settings.Add("Application:Version", $"{DateTime.UtcNow.Day}.{DateTime.UtcNow.Millisecond}");
                Settings.Add("Application:Environment", $"Development");
                var notprovidedKeys = requiredDevSettings.Except(requiredDevSettings.Intersect((IEnumerable<string>)Settings.Keys)).ToList();
                if (notprovidedKeys.Any())
                {
                    throw new KeyNotFoundException($"No such keys:[{string.Join(", ", notprovidedKeys)}] in dev-config.json");
                }
            }
            else if (appEnv == "STAGING" || appEnv == "PRODUCTION")
            {
                var variables = (IDictionary<string, string>)Environment.GetEnvironmentVariables();
                var mapKeysList = stagingProdSettingsMap.Keys.ToList();
                var notprovidedKeys = mapKeysList.Except(mapKeysList.Intersect(variables.Keys));
                if (notprovidedKeys.Any())
                {
                    throw new KeyNotFoundException($"Not such keys:[{string.Join(", ", notprovidedKeys)}] provided in build environment!");
                }
                foreach (var setting in stagingProdSettingsMap)
                {
                    Settings.Add(setting.Value, variables[setting.Key]);
                }
            }
        }

        private string ToCamelCase(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }
            var x = s.Replace("_", "");
            if (x.Length == 0) return null;
            x = Regex.Replace(x, "([A-Z])([A-Z]+)($|[A-Z])",
                m => m.Groups[1].Value + m.Groups[2].Value.ToLower() + m.Groups[3].Value);
            return char.ToUpper(x[0]) + x.Substring(1);
        }

        public override XmlNode ProcessRawXml(XmlNode rawXml)
        {
            foreach (DictionaryEntry setting in Settings)
            {

                var pair = (Key: setting.Key.ToString(), Value: setting.Value.ToString());

                if (rawXml.HasChildNodes
                    && rawXml.SelectSingleNode($"add[@key='{pair.Key}']") != null)
                {
                    rawXml.SelectSingleNode($"add[@key='{pair.Key}']")
                        .Attributes["value"].Value = pair.Value;
                }

            }

            return rawXml;

        }
    }
}