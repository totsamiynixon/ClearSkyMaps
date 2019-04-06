using System.Collections.Generic;
using System.Web.Mvc;
using Web.Areas.Admin.Enums;
using Web.Areas.Admin.Filters;
using Web.Areas.Admin.Models;

namespace Web.Areas.Admin.Controllers
{
    [AdminAreaHandleException]
    [RouteArea("Admin")]
    [Authorize]
    public abstract class BaseController : Controller
    {
        public void ShowAlert(AlertTypes type, string message)
        {
            List<Alert> alerts = new List<Alert>();
            if ((TempData["Alerts"] as List<Alert>) != null)
            {
                alerts = (TempData["Alerts"] as List<Alert>);
            }
            alerts.Add(new Alert
            {
                Message = message,
                Type = type
            });
            TempData["Alerts"] = alerts;
        }

        public List<Alert> GetAlerts()
        {
            if ((TempData["Alerts"] as List<Alert>) != null)
            {
                return (TempData["Alerts"] as List<Alert>);
            }
            return new List<Alert>();
        }
    }
}