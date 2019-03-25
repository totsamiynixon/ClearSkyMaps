using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Web.Areas.Admin;
using Web.Areas.PWA;

namespace Web.Areas
{
    public static class AreaConfig
    {
        public static void RegisterAreas()
        {
            // 
            // Admin area . . .

            var adminArea = new AdminAreaRegistration();
            var adminAreaContext = new AreaRegistrationContext(adminArea.AreaName, RouteTable.Routes);
            adminArea.RegisterArea(adminAreaContext);


            // 
            // PWA area . . .

            var pwaArea = new PWAAreaRegistration();
            var pwaAreaContext = new AreaRegistrationContext(pwaArea.AreaName, RouteTable.Routes);
            pwaArea.RegisterArea(pwaAreaContext);
        }
    }
}