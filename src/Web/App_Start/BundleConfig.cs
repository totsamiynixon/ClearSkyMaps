using System.Web;
using System.Web.Optimization;

namespace Web
{
    public class BundleConfig
    {
        // Дополнительные сведения об объединении см. на странице https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                     "~/Scripts/jquery-3.3.1.min.js",
                     "~/Scripts/popper.js",
                     "~/Scripts/bootstrap-material-design.js",
                     "~/Scripts/Chart.bundle.min.js",
                     "~/Scripts/moment.js",
                     "~/Scripts/vue.js",
                     "~/Scripts/vue-router.js",
                     "~/Scripts/front/vue-filters.js",
                     "~/Scripts/jquery.signalR-2.2.2.min.js",
                     "~/Scripts/front/main.js",
                     "~/Scripts/front/readings.js",
                     "~/Scripts/front/offline.js",
                     "~/Scripts/front/app.js"
                     ));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                      "~/Content/bootstrap-material-design.css",
                      "~/Content/site.css"));
        }
    }
}
