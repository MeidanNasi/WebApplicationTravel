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
        public ActionResult Index(string search)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var s = from c in db.Accounts
                    select c;
            if (!String.IsNullOrEmpty(search))
            {
                s = s.Where(x => x.UserName.Equals(search));
                return View(s.ToList());
            }
            return View(db.Accounts.ToList());
        }

        // GET: Accounts/Details/5
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
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AccountId,UserName,Password,Admin")] Account account)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
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
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
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
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
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
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
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
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Account account = db.Accounts.Find(id);
            IEnumerable<Reservation> q = db.Reservations.Where(c => c.AccountId==id);
            foreach (Reservation c in q)
            {
                db.Reservations.Remove(c);
            }
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
            var user = db.Accounts.SingleOrDefault(u => u.UserName == account.UserName && u.Password == account.Password);
            if (user != null)
            {
                Session["UserName"] = user.UserName.ToString();
                Session["Admin"] = null;
                if (user.Admin)
                    Session["Admin"] = "admin";
                Session["AccountId"] = user.AccountId.ToString();
                Session["wrong password"] = null;
                return RedirectToAction("LoggedIn");
            }
            else
            {
                Session["wrong password"] = true;
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
                return RedirectToAction("Login");
            }
        }

        public ActionResult Logout()
        {
            Session["UserName"] = null;
            Session["Admin"] = null;
            return RedirectToAction("Index", "Home");
        }
      
    }

}
