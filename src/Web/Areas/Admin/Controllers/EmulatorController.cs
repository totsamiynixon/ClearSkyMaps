using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Data;

namespace Web.Areas.Admin.Controllers
{
    public class EmulatorController : Controller
    {
        private readonly DataContext _context;
        public EmulatorController()
        {
            _context = new DataContext();
        }
        // GET: Admin/Emulator
        public ActionResult Index()
        {
            return View();
        }
    }
}