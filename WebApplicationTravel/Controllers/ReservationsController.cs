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
    public class ReservationsController : Controller
    {
        private MSGDBContext db = new MSGDBContext();

        // GET: Reservations
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var reservations = db.Reservations.Include(r => r.Account);
            return View(reservations.ToList());
        }

        // GET: Reservations/Details/5
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
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // GET: Reservations/Create
        public ActionResult Create()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "AccountId", "UserName");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReservationId,AccountId")] Reservation reservation)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                db.Reservations.Add(reservation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccountId = new SelectList(db.Accounts, "AccountId", "UserName", reservation.AccountId);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
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
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "AccountId", "UserName", reservation.AccountId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReservationId,AccountId")] Reservation reservation)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "AccountId", "UserName", reservation.AccountId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
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
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Reservation reservation = db.Reservations.Find(id);
            db.Reservations.Remove(reservation);
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

        public ActionResult calcPath(string from, string dest,string date, string submit)
        {
            if (Session["UserName"] != null)
            {
                var c = db.Cities.SingleOrDefault(s => s.CityName == from);
                var d = db.Cities.SingleOrDefault(s => s.CityName == dest);
                Session["wrong source"] = null;
                Session["wrong dest"] = null;
                Session["no path"] = null;
                if (c == null)
                {
                    Session["wrong source"] = true;
                    return RedirectToAction("Index", "Home");
                }
                if (d == null)
                {
                    Session["wrong dest"] = true;
                    return RedirectToAction("Index", "Home");
                }
                Account a = db.Accounts.Find(int.Parse(Session["AccountId"].ToString()));
                ReservationBuilder r;
                if (submit.Contains("cheapest"))
                {
                    LinkedList<string> path = calcCheapestWay(from, dest);
                    if (path.Count() ==0 )
                    {
                        Session["no path"] = true;
                        return RedirectToAction("Index", "Home");
                    }
                    r = new ReservationBuilder(path);
                }
                else
                {
                    LinkedList<string> path = calcFastestWay(from, dest);
                    if (path.Count() == 0)
                    {
                        Session["no path"] = true;
                        return RedirectToAction("Index", "Home");
                    }
                    r = new ReservationBuilder(path);
                }
                r.bulidReservation();
                LinkedList<string> res = r.updateReservation();
                if (res != null)
                {
                    Reservation reservation = new Reservation();
                    reservation.AccountId = a.AccountId;
                    reservation.Account = a;
                    reservation.Departure = date;
                    foreach(string s in res)
                    {
                        reservation.TheReservation += s+ " | ";
                    }
                    db.Reservations.Add(reservation);
                    db.SaveChanges();
                    return RedirectToAction("ShowReservation", "Reservations", reservation);
                }
                else return View();//pop up no path msg               
            }
            else
            {
                return RedirectToAction("login","Accounts"); //need to show "please log in first"+button
            }
        }
        public LinkedList<string> calcCheapestWay(string source, string dest)
        {
            BestFirstSearch bfs = new BestFirstSearch();
            bfs.BuildGraph();
            bfs.Search(source, dest, "price");
            return bfs.path;
        }
        public LinkedList<string> calcFastestWay(string source, string dest)
        {
            BestFirstSearch bfs = new BestFirstSearch();
            bfs.BuildGraph();
            bfs.Search(source, dest, "time");
            return bfs.path;
        }

        public ActionResult Profile()
        {
            Account a = db.Accounts.Find(int.Parse(Session["AccountId"].ToString()));
            var accountReservations = db.Reservations.Where(c => c.AccountId == a.AccountId);
            return View(accountReservations.ToList());
        }
        public ActionResult ShowReservation(Reservation r)
        {
            ViewBag.res = r;
            return View();
        }
        public ActionResult DoNotOrder(int? id)
        {
            Reservation r = db.Reservations.Find(id);
            db.Reservations.Remove(r);
            db.SaveChanges(); 
            return RedirectToAction("Index", "Home");
        }
    }
}
