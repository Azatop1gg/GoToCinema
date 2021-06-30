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
    [Authorize(Roles ="admin")]
    public class RowsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Rows
        [AllowAnonymous]
        public ActionResult Index()
        {
            var rows = db.Rows.Include(r => r.Hall);
            return View(rows.ToList());
        }

        [AllowAnonymous]
        // GET: Rows/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Row row = db.Rows.Include(x=>x.Hall).FirstOrDefault(x=>x.Id == id);
            if (row == null)
            {
                return HttpNotFound();
            }
            return View(row);
        }

        // GET: Rows/Create
        public ActionResult Create()
        {
            ViewBag.HallId = new SelectList(db.Halls, "Id", "Name");
            return View();
        }

        // POST: Rows/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Row row)
        {
            if (ModelState.IsValid)
            {
                db.Rows.Add(row);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HallId = new SelectList(db.Halls, "Id", "Name", row.HallId);
            return View(row);
        }

        // GET: Rows/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Row row = db.Rows.Find(id);
            if (row == null)
            {
                return HttpNotFound();
            }
            ViewBag.HallId = new SelectList(db.Halls, "Id", "Name", row.HallId);
            return View(row);
        }

        // POST: Rows/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Row row)
        {
            if (ModelState.IsValid)
            {
                db.Entry(row).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HallId = new SelectList(db.Halls, "Id", "Name", row.HallId);
            return View(row);
        }

        // GET: Rows/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Row row = db.Rows.Include(x => x.Hall).FirstOrDefault(x => x.Id == id);

            if (row == null)
            {
                return HttpNotFound();
            }
            return View(row);
        }

        // POST: Rows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Row row = db.Rows.Find(id);
            var seats = db.Seats.Where(x => x.RowId == row.Id);
            if(seats != null)
            {
                foreach(var seat in seats)
                {
                    db.Seats.Remove(seat);
                }
            }
            db.Rows.Remove(row);
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
