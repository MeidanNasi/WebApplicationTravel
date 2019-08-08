using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplicationTravel.Models;

namespace WebApplicationTravel.Controllers
{
    public class ConnectionsController : Controller
    {
        private MSGDBContext db = new MSGDBContext();

        // GET: Connections
        public ActionResult Index(string search)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var s = from c in db.Connections
                    select c;
            if (!String.IsNullOrEmpty(search))
            {
                s = s.Where(x => x.SourceCity.CityName.Equals(search)).Include(c => c.SourceCity);
                return View(s.ToList());
            }
            var connections = db.Connections.Include(c => c.DestCity).Include(c => c.SourceCity);
            return View(connections.ToList());
        }

        // GET: Connections/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Connections connections = db.Connections.Find(id);
            if (connections == null)
            {
                return HttpNotFound();
            }
            return View(connections);
        }

        // GET: Connections/Create
        public ActionResult Create()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.DestCityId = new SelectList(db.Cities, "CityId", "CityName");
            ViewBag.SourceCityId = new SelectList(db.Cities, "CityId", "CityName");
            return View();
        }

        // POST: Connections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ConnectionsId,SourceCityId,DestCityId,FlightDuration,CarDuration,FlightPrice,CarPrice,CarAvailabilty,FlightAvailabilty")] Connections connections)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                PriceAndDurationCalc(connections);
                db.Connections.Add(connections);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DestCityId = new SelectList(db.Cities, "CityId", "CityName", connections.DestCityId);
            ViewBag.SourceCityId = new SelectList(db.Cities, "CityId", "CityName", connections.SourceCityId);
            return View(connections);
        }

        // GET: Connections/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Connections connections = db.Connections.Find(id);
            if (connections == null)
            {
                return HttpNotFound();
            }
            ViewBag.DestCityId = new SelectList(db.Cities, "CityId", "CityName", connections.DestCityId);
            ViewBag.SourceCityId = new SelectList(db.Cities, "CityId", "CityName", connections.SourceCityId);
            return View(connections);
        }

        // POST: Connections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ConnectionsId,SourceCityId,DestCityId,FlightDuration,CarDuration,FlightPrice,CarPrice,CarAvailabilty,FlightAvailabilty")] Connections connections)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                PriceAndDurationCalc(connections);
                db.Entry(connections).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DestCityId = new SelectList(db.Cities, "CityId", "CityName", connections.DestCityId);
            ViewBag.SourceCityId = new SelectList(db.Cities, "CityId", "CityName", connections.SourceCityId);
            return View(connections);
        }

        // GET: Connections/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Connections connections = db.Connections.Find(id);
            if (connections == null)
            {
                return HttpNotFound();
            }
            return View(connections);
        }

        // POST: Connections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Connections connections = db.Connections.Find(id);
            db.Connections.Remove(connections);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public void PriceAndDurationCalc(Connections connection)
        {
            City source = db.Cities.Find(connection.SourceCityId);
            City dest = db.Cities.Find(connection.DestCityId);

            double x0 = source.Longitude;
            double x1 = dest.Longitude;
            double y0 = source.Latitude;
            double y1 = dest.Latitude;
            double distance = Math.Sqrt(Math.Pow(x0 - x1, 2) + Math.Pow(y0 - y1, 2));
            if (connection.FlightAvailabilty)
            {
                connection.FlightDuration = System.Math.Round(distance / 5, 2);
                connection.FlightPrice = connection.FlightDuration * source.FlightPriceKey;
            }
            else
            {
                connection.FlightDuration = 0;
                connection.FlightPrice = 0;
            }
            if (connection.CarAvailabilty)
            {
                connection.CarDuration = System.Math.Round(distance / 0.8, 2);
                connection.CarPrice = connection.CarDuration * source.CarRentalPriceKey;
            }
            else
            {
                connection.CarDuration = 0;
                connection.CarPrice = 0;
            }

        }
        public ActionResult ConnectionSearch(string sourceCity, string flightDuration, string flightPrice)
        {
            var cons = from c in db.Connections
                       select c;
            if (!String.IsNullOrEmpty(flightDuration))
            {
                var duration = Convert.ToDouble(flightDuration);
                cons = cons.Where(p => p.FlightDuration <= duration || flightDuration == null || flightDuration == "");
            }
            if (!String.IsNullOrEmpty(flightPrice))
            {
                var price = Convert.ToDouble(flightPrice);
                cons = cons.Where(p => p.FlightPrice <= price || flightPrice == null || flightPrice == "");
            }
            cons = cons.Where(p => p.SourceCity.CityName.ToLower().Contains(sourceCity.ToLower()) || sourceCity == null || sourceCity == "");
            
            

            //if (!String.IsNullOrEmpty(sourceCity) /*|| !String.IsNullOrEmpty(flightDuration) || !String.IsNullOrEmpty(flightPrice)*/)
            //{
            //    cons = db.Connections.Where(c => c.SourceCity.CityName.Equals(sourceCity) && c.FlightDuration <=flightDuration && c.FlightPrice <=flightPrice);

            //}
            return View(cons.ToList());
        }

    }
}
   
