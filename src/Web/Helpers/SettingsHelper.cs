using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Web.Helpers
{
    public static class SettingsHelper
    {
        public static bool EmulationEnabled => bool.Parse(ConfigurationManager.AppSettings["Emulation:Enabled"].ToString());

        public static string FirebaseCloudMessagingServerKey => ConfigurationManager.AppSettings["FirebaseCloudMessaging:ServerKey"].ToString();

        public static string FirebaseCloudMessagingMessagingSenderId => ConfigurationManager.AppSettings["FirebaseCloudMessaging:MessagingSenderId"].ToString();

        public static string ApplicationVersion => ConfigurationManager.AppSettings["Application:Version"].ToString();

        public static string ApplicationEnvironment => ConfigurationManager.AppSettings["Application:Environment"].ToString();
    }
}