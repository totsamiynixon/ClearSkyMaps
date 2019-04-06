using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Areas.Admin.Controllers
{
    [Authorize]
    [RouteArea("Admin")]
    public class ErrorController : Controller
    {
        public ActionResult Index(HttpException exception)
        {
            return View(exception);
        }
    }
}