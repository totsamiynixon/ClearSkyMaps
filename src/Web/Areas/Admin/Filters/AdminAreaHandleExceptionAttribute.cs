using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Web.Areas.Admin.Controllers;
using Web.Helpers;

namespace Web.Areas.Admin.Filters
{
    public class AdminAreaHandleExceptionAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is HttpException)
            {
                IController newController = new ErrorController();
                var routeData = new RouteData();
                routeData.Values.Add("controller", "Error");
                routeData.Values.Add("action", "Index");
                routeData.Values.Add("area", "Admin");
                routeData.Values.Add("exception", filterContext.Exception);
                var newRequestContext = new RequestContext(new HttpContextWrapper(HttpContext.Current), routeData);
                newController.Execute(newRequestContext);
                filterContext.ExceptionHandled = true;
                return;
            }
            base.OnException(filterContext);
        }
    }
}