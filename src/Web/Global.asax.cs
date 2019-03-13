using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Web.Helpers;
using Web.Resolvers;

namespace Web
{
    public class WebApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            LoggerHelper.InitLogger();
            SettingsHelper.InitConfig();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RegisterSignalrSerializer();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
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
