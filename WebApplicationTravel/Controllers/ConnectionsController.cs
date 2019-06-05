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
        public ActionResult Index()
        {
            var connections = db.Connections.Include(c => c.DestCity).Include(c => c.SourceCity);
            return View(connections.ToList());
        }

        // GET: Connections/Details/5
        public ActionResult Details(int? id)
        {
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

            double x0 = source.Coordinate.X;
            double x1 = dest.Coordinate.X;
            double y0 = source.Coordinate.Y;
            double y1 = dest.Coordinate.Y;
            double distance = Math.Sqrt(Math.Pow(x0 - x1, 2) + Math.Pow(y0 - y1, 2));
            if (connection.FlightAvailabilty)
            {
                connection.FlightDuration =System.Math.Round( distance / 5, 2);
                connection.FlightPrice = connection.FlightDuration * source.FlightPriceKey;
            }
            else
            {
                connection.FlightDuration = null;
                connection.FlightPrice = null;
            }
            if (connection.CarAvailabilty)
            {
                connection.CarDuration = System.Math.Round(distance / 0.8, 2);
                connection.CarPrice = connection.CarDuration * source.CarRentalPriceKey;
            }
            else
            {
                connection.CarDuration = null;
                connection.CarPrice = null;
            }
            
        }
     
    }
}
