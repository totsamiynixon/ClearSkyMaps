using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.ViewEngines
{
    public class AreaAwareViewEngine : BaseAreaAwareViewEngine
    {
        public AreaAwareViewEngine()
        {
            MasterLocationFormats = new string[]
            {
            "~/Areas/{2}/Views/{1}/{0}.master",
            "~/Areas/{2}/Views/{1}/{0}.cshtml",
            "~/Areas/{2}/Views/Shared/{0}.master",
            "~/Areas/{2}/Views/Shared/{0}.cshtml",
            "~/Views/{1}/{0}.master",
            "~/Views/{1}/{0}.cshtml",
            "~/Views/Shared/{0}.master",
            "~/Views/Shared/{0}.cshtml",
            };
            ViewLocationFormats = new string[]
            {
            "~/Areas/{2}/Views/{1}/{0}.aspx",
            "~/Areas/{2}/Views/{1}/{0}.ascx",
            "~/Areas/{2}/Views/{1}/{0}.cshtml",
            "~/Areas/{2}/Views/Shared/{0}.aspx",
            "~/Areas/{2}/Views/Shared/{0}.ascx",
            "~/Areas/{2}/Views/Shared/{0}.cshtml",
            "~/Views/{1}/{0}.aspx",
            "~/Views/{1}/{0}.ascx",
            "~/Views/{1}/{0}.cshtml",
            "~/Views/Shared/{0}.aspx",
            "~/Views/Shared/{0}.ascx",
            "~/Views/Shared/{0}.cshtml",
            };
            PartialViewLocationFormats = ViewLocationFormats;
        }

        protected override IView CreatePartialView(
            ControllerContext controllerContext, string partialPath)
        {
            if (partialPath.EndsWith(".cshtml"))
                return new System.Web.Mvc.RazorView(controllerContext, partialPath, null, false, null);
            else
                return new WebFormView(controllerContext, partialPath);
        }

        protected override IView CreateView(ControllerContext controllerContext,
            string viewPath, string masterPath)
        {
            if (viewPath.EndsWith(".cshtml"))
                return new RazorView(controllerContext, viewPath, masterPath, false, null);
            else
                return new WebFormView(controllerContext, viewPath, masterPath);
        }
    }
}