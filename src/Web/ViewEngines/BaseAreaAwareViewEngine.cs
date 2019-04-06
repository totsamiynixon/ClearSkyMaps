using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.ViewEngines
{
    public abstract class BaseAreaAwareViewEngine : VirtualPathProviderViewEngine
    {
        private static readonly string[] EmptyLocations = { };

        public override ViewEngineResult FindView(
            ControllerContext controllerContext, string viewName,
            string masterName, bool useCache)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (string.IsNullOrEmpty(viewName))
            {
                throw new ArgumentNullException(viewName,
                    "Value cannot be null or empty.");
            }

            string area = getArea(controllerContext);
            return FindAreaView(controllerContext, area, viewName,
                masterName, useCache);
        }

        public override ViewEngineResult FindPartialView(
            ControllerContext controllerContext, string partialViewName,
            bool useCache)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (string.IsNullOrEmpty(partialViewName))
            {
                throw new ArgumentNullException(partialViewName,
                    "Value cannot be null or empty.");
            }

            string area = getArea(controllerContext);
            return FindAreaPartialView(controllerContext, area,
                partialViewName, useCache);
        }

        protected virtual ViewEngineResult FindAreaView(
            ControllerContext controllerContext, string areaName, string viewName,
            string masterName, bool useCache)
        {
            string controllerName =
                controllerContext.RouteData.GetRequiredString("controller");
            string[] searchedViewPaths;
            string viewPath = GetPath(controllerContext, ViewLocationFormats,
                "ViewLocationFormats", viewName, controllerName, areaName, "View",
                useCache, out searchedViewPaths);
            string[] searchedMasterPaths;
            string masterPath = GetPath(controllerContext, MasterLocationFormats,
                "MasterLocationFormats", masterName, controllerName, areaName,
                "Master", useCache, out searchedMasterPaths);
            if (!string.IsNullOrEmpty(viewPath) &&
                (!string.IsNullOrEmpty(masterPath) ||
                  string.IsNullOrEmpty(masterName)))
            {
                return new ViewEngineResult(CreateView(controllerContext, viewPath,
                    masterPath), this);
            }
            return new ViewEngineResult(
                searchedViewPaths.Union<string>(searchedMasterPaths));
        }

        protected virtual ViewEngineResult FindAreaPartialView(
            ControllerContext controllerContext, string areaName,
            string viewName, bool useCache)
        {
            string controllerName =
                controllerContext.RouteData.GetRequiredString("controller");
            string[] searchedViewPaths;
            string partialViewPath = GetPath(controllerContext,
                ViewLocationFormats, "PartialViewLocationFormats", viewName,
                controllerName, areaName, "Partial", useCache,
                out searchedViewPaths);
            if (!string.IsNullOrEmpty(partialViewPath))
            {
                return new ViewEngineResult(CreatePartialView(controllerContext,
                    partialViewPath), this);
            }
            return new ViewEngineResult(searchedViewPaths);
        }

        protected string CreateCacheKey(string prefix, string name,
            string controller, string area)
        {
            return string.Format(CultureInfo.InvariantCulture,
                ":ViewCacheEntry:{0}:{1}:{2}:{3}:{4}:",
                base.GetType().AssemblyQualifiedName,
                prefix, name, controller, area);
        }

        protected string GetPath(ControllerContext controllerContext,
            string[] locations, string locationsPropertyName, string name,
            string controllerName, string areaName, string cacheKeyPrefix,
            bool useCache, out string[] searchedLocations)
        {
            searchedLocations = EmptyLocations;
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }
            if ((locations == null) || (locations.Length == 0))
            {
                throw new InvalidOperationException(string.Format("The property " +
                    "'{0}' cannot be null or empty.", locationsPropertyName));
            }
            bool isSpecificPath = IsSpecificPath(name);
            string key = CreateCacheKey(cacheKeyPrefix, name,
                isSpecificPath ? string.Empty : controllerName,
                isSpecificPath ? string.Empty : areaName);
            if (useCache)
            {
                string viewLocation = ViewLocationCache.GetViewLocation(
                    controllerContext.HttpContext, key);
                if (viewLocation != null)
                {
                    return viewLocation;
                }
            }
            if (!isSpecificPath)
            {
                return GetPathFromGeneralName(controllerContext, locations, name,
                    controllerName, areaName, key, ref searchedLocations);
            }
            return GetPathFromSpecificName(controllerContext, name, key,
                ref searchedLocations);
        }

        protected string GetPathFromGeneralName(ControllerContext controllerContext,
            string[] locations, string name, string controllerName,
            string areaName, string cacheKey, ref string[] searchedLocations)
        {
            string virtualPath = string.Empty;
            searchedLocations = new string[locations.Length];
            for (int i = 0; i < locations.Length; i++)
            {
                if (string.IsNullOrEmpty(areaName) && locations[i].Contains("{2}"))
                {
                    continue;
                }
                string testPath = string.Format(CultureInfo.InvariantCulture,
                    locations[i], name, controllerName, areaName);
                if (FileExists(controllerContext, testPath))
                {
                    searchedLocations = EmptyLocations;
                    virtualPath = testPath;
                    ViewLocationCache.InsertViewLocation(
                        controllerContext.HttpContext, cacheKey, virtualPath);
                    return virtualPath;
                }
                searchedLocations[i] = testPath;
            }
            return virtualPath;
        }

        protected string GetPathFromSpecificName(
            ControllerContext controllerContext, string name, string cacheKey,
            ref string[] searchedLocations)
        {
            string virtualPath = name;
            if (!FileExists(controllerContext, name))
            {
                virtualPath = string.Empty;
                searchedLocations = new string[] { name };
            }
            ViewLocationCache.InsertViewLocation(controllerContext.HttpContext,
                cacheKey, virtualPath);
            return virtualPath;
        }


        protected string getArea(ControllerContext controllerContext)
        {
            // First try to get area from a RouteValue override, like one specified in the Defaults arg to a Route.
            object areaO;
            controllerContext.RouteData.Values.TryGetValue("area", out areaO);

            // If not specified, try to get it from the Controller's namespace
            if (areaO != null)
                return (string)areaO;

            string namespa = controllerContext.Controller.GetType().Namespace;
            int areaStart = namespa.IndexOf("Areas.");
            if (areaStart == -1)
                return null;

            areaStart += 6;
            int areaEnd = namespa.IndexOf('.', areaStart + 1);
            string area = namespa.Substring(areaStart, areaEnd - areaStart);
            return area;
        }

        protected static bool IsSpecificPath(string name)
        {
            char ch = name[0];
            if (ch != '~')
            {
                return (ch == '/');
            }
            return true;
        }
    }
}