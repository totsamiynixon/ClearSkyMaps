using System.Web;
using System.Web.Optimization;

namespace Web
{
    public class BundleConfig
    {
        // Дополнительные сведения об объединении см. на странице https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //PWA
            bundles.Add(new ScriptBundle("~/bundles/pwa/js").Include(
                     "~/Areas/PWA/Scripts/jquery-3.3.1.min.js",
                     "~/Areas/PWA/Scripts/popper.js",
                     "~/Areas/PWA/Scripts/bootstrap-material-design.js",
                     "~/Areas/PWA/Scripts/Chart.bundle.min.js",
                     "~/Areas/PWA/Scripts/moment.js",
                     "~/Areas/PWA/Scripts/vue.js",
                     "~/Areas/PWA/Scripts/vue-router.js",
                     "~/Areas/PWA/Scripts/front/vue-filters.js",
                     "~/Areas/PWA/Scripts/jquery.signalR-2.2.2.min.js",
                     "~/Areas/PWA/Scripts/front/main.js",
                     "~/Areas/PWA/Scripts/front/readings.js",
                     "~/Areas/PWA/Scripts/front/offline.js",
                     "~/Areas/PWA/Scripts/front/app.js"
                     ));

            bundles.Add(new StyleBundle("~/bundles/pwa/css").Include(
                      "~/Areas/PWA/Content/bootstrap-material-design.css",
                      "~/Areas/PWA/Content/site.css"));


            //Admin
            bundles.Add(new ScriptBundle("~/bundles/admin/js").Include(
                     "~/Areas/Admin/Theme/js/core/jquery.min.js",
                     "~/Areas/Admin/Theme/js/core/popper.min.js",
                     "~/Areas/Admin/Theme/js/core/bootstrap.min.js",
                     "~/Areas/Admin/Theme/js/plugins/perfect-scrollbar.jquery.min.js",
                     "~/Areas/Admin/Theme/js/plugins/chartjs.min.js",
                     "~/Areas/Admin/Theme/js/plugins/bootstrap-notify.js",
                     "~/Areas/Admin/Theme/js/paper-dashboard.min.js?v=2.0.0",
                     "~/Areas/Admin/Scripts/libs/vue.js"
                     ));

            bundles.Add(new StyleBundle("~/bundles/admin/css").Include(
                      "~/Areas/Admin/Theme/css/bootstrap.min.css",
                      "~/Areas/Admin/Theme/css/paper-dashboard.min.css",
                      "~/Areas/Admin/Content/css/site.css"));
        }
    }
}
