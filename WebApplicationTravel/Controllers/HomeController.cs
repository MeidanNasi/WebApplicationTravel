using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationTravel.Models;

namespace WebApplicationTravel.Controllers
{
    public class HomeController : Controller
    {
        private MSGDBContext db = new MSGDBContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult ComingSoon()
        {
            ViewBag.Message = "Coming Soon.";

            return View();
        }
        public ActionResult GoogleMaps()
        {
            ViewBag.Message = "GoogleMaps.";

            return View();
        }

        public JsonResult ResGoogleMap()
        {
            var data = from r in db.Cities
                       select new Point
                       {
                           X = r.Coordinate.X,
                           Y = r.Coordinate.Y
                       };
            //viewmodel.resMap = data.ToList();
            List<Point> resList = data.ToList();
            return Json(resList);
        }

    }
}