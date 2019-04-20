using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Web.Areas;
using Web.Helpers;
using Web.Resolvers;
using Web.ViewEngines;

namespace Web
{
    public class WebApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            LoggerHelper.InitLogger();
            SettingsHelper.InitConfig();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RegisterSignalrSerializer();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AreaConfig.RegisterAreas();
            System.Web.Mvc.ViewEngines.Engines.Clear();
            System.Web.Mvc.ViewEngines.Engines.Add(new AreaAwareViewEngine());
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DatabaseHelper.ReinitializeDb();
        }

        protected void Application_Error()
        {
            Debug.WriteLine("Application_Error");
            var exception = Server.GetLastError();

            LoggerHelper.Log.Error("Apllication_Error", exception);

            Response.TrySkipIisCustomErrors = true;
            Response.Clear();
            Server.ClearError();

            if (exception is HttpException)
            {
                Response.StatusCode = (exception as HttpException).GetHttpCode();
                Response.StatusDescription = (exception as HttpException).GetHtmlErrorMessage();
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Response.StatusDescription = "Something wrong on our side!";
            }
        }


        private void RegisterSignalrSerializer()
        {
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new SignalRContractResolver();
            var serializer = JsonSerializer.Create(settings);
            GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);
        }
    }
}
