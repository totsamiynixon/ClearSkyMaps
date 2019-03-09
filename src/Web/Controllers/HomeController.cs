using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Results;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Readings()
        {
            return View();
        }
    }
}
