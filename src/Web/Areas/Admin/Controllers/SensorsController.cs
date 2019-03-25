using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Areas.Admin.Controllers
{
    public class SensorsController : BaseController
    {
        // GET: Admin/Sensors
        public ActionResult Index()
        {
            return View();
        }
    }
}