using System.Web.Mvc;

namespace Web.Areas.PWA
{
    public class PWAAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "PWA";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "PWA_Manifest",
                url: "manifest.json",
                defaults: new { controller = "Home", action = "ManifestJsonAsync" }
            );

            context.MapRoute(
                name: "PWA_ServiceWorker",
                url: "service-worker.js",
                defaults: new { controller = "Home", action = "ServiceWorkerJsAsync" }
            );

            context.MapRoute(
               name: "pwa-fallback",
               url: "{*url}",
               defaults: new { controller = "Home", action = "Index" }
            );

            context.MapRoute(
                "PWA_default",
                "{controller}/{action}/{id}",
                new { context = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}