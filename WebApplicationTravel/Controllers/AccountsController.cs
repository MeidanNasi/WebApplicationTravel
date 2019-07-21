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
    public class AccountsController : Controller
    {
        private MSGDBContext db = new MSGDBContext();

        // GET: Accounts
        [HttpGet]
        public ActionResult Index()
        {
            return View(db.Accounts.ToList());
        }

        // GET: Accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: Accounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AccountId,UserName,Password,Admin")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(account);
        }

        // GET: Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AccountId,UserName,Password,Admin")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(account);
        }

        // GET: Accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Account account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
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
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Account account)
        {
            if (ModelState.IsValid)
            {
                db.Accounts.Add(account);
                db.SaveChanges();
                ModelState.Clear();
                ViewBag.Message = account.UserName + " Succesefully registered!";
            }

            return RedirectToAction("Index", "Home");
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Account account)
        {
            var user = db.Accounts.Single(u => u.UserName == account.UserName && u.Password == account.Password);
            if (user != null)
            {
                Session["UserName"] = user.UserName.ToString();
                Session["Admin"] = user.Admin;
                Session["AccountId"] = user.AccountId.ToString();
                Session["Reservations"] = user.Reservations;
                return RedirectToAction("LoggedIn");
            }
            else
            {
                ModelState.AddModelError("", "User name or password is wrong");
            }

            return View();
        }

        public ActionResult LoggedIn()
        {
            if (Session["UserName"] != null)
            {
                //return View();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("login");
            }
        }

        public ActionResult Logout()
        {
            Session["UserName"] = null;
            return RedirectToAction("Index", "Home");
        }
        public ActionResult calcPath(string from, string dest, string submit)
        {
            if (Session["UserName"] != null)
            {
                Account a = db.Accounts.Find(int.Parse(Session["AccountId"].ToString()));
                Reservation r;
                if (submit.Contains("cheapest"))
                {
                    r = new Reservation(calcCheapestWay(from, dest));
                }
                else
                {
                    r = new Reservation(calcFastestWay(from, dest));
                }
                r.bulidReservation();
                //a.Reservations.AddFirst(r);
                db.SaveChanges();

                return RedirectToAction("Index", "Accounts");
            }
            else
            {
                return null; //need to show "please log in first"+button
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
    }

}
