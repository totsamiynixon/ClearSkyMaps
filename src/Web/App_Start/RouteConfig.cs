using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.RouteExistingFiles = true;

            routes.MapRoute(
                name: "Manifest",
                url: "manifest.json",
                defaults: new { controller = "FileTransfer", action = "ManifestJsonAsync" }
            );

            routes.MapRoute(
                name: "ServiceWorker",
                url: "service-worker.js",
                defaults: new { controller = "FileTransfer", action = "ServiceWorkerJsAsync" }
            );

            routes.MapRoute(
               name: "spa-fallback",
               url: "{*url}",
               defaults: new { controller = "PWA", action = "Index" }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "PWA", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
