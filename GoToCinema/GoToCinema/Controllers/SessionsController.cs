using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GoToCinema.Models;
using GoToCinema.Models.DomainModels;

namespace GoToCinema.Controllers
{
    [Authorize]
    public class SessionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Sessions
        [AllowAnonymous]
        public ActionResult Index()
        {
            var sessions = db.Sessions.Include(s => s.Hall).Include(s => s.Movie).Include(c=>c.Hall.Cinema);
            return View(sessions.ToList());
        }

        // GET: Sessions/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = db.Sessions.Include(x=>x.Hall).Include(x=>x.Movie).FirstOrDefault(x=>x.Id == id);
            
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // GET: Sessions/Create
        public ActionResult Create()
        {
            ViewBag.HallId = new SelectList(db.Halls, "Id", "Name");
            ViewBag.MovieId = new SelectList(db.Movies, "Id", "Name");
            return View();
        }

        // POST: Sessions/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Session session, TimeSpan startTime, TimeSpan timeofend)
        {
            ViewBag.HallId = new SelectList(db.Halls, "Id", "Name", session.HallId);
            ViewBag.MovieId = new SelectList(db.Movies, "Id", "Name", session.MovieId);
            session.Date = session.Date + startTime;
            session.EndTime = session.EndTime + timeofend;
            if (ModelState.IsValid)
            {
                if(session.Date > session.EndTime)
                {
                    ModelState.AddModelError("", "Не правильно поставлено время начала и конца сеанса!");

                    return View(session);
                }
                db.Sessions.Add(session);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(session);
        }

        // GET: Sessions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            ViewBag.HallId = new SelectList(db.Halls, "Id", "Name", session.HallId);
            ViewBag.MovieId = new SelectList(db.Movies, "Id", "Name", session.MovieId);
            return View(session);
        }

        // POST: Sessions/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Session session, DateTime time)
        {
            if (ModelState.IsValid)
            {
                session.Date = time;
                db.Entry(session).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HallId = new SelectList(db.Halls, "Id", "Name", session.HallId);
            ViewBag.MovieId = new SelectList(db.Movies, "Id", "Name", session.MovieId);
            return View(session);
        }

        // GET: Sessions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = db.Sessions.Include(x => x.Hall).Include(x => x.Movie).FirstOrDefault(x => x.Id == id);

            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // POST: Sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Session session = db.Sessions.Find(id);
            var sessionSeats = db.SessionSeats.Where(x => x.SessionId == session.Id);
            if(sessionSeats != null)
            {
                foreach(var sessionSeat in sessionSeats)
                {
                    db.SessionSeats.Remove(sessionSeat);
                }
            }
            db.Sessions.Remove(session);
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
    }
}
