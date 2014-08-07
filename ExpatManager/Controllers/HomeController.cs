using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExpatManager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to Expatriate Management";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Help()
        {
            ViewBag.Message = "Expatriate Management Help";

            return View();
        }

        public ActionResult Report()
        {
            
            ViewBag.Message = "Expatriate Management Reports";

            return View();
        }
    }
}
