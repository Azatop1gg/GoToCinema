using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using GoToCinema.Models;
using GoToCinema.Models.DomainModels;

namespace GoToCinema.Controllers
{
    [Authorize(Roles ="admin")]
    public class HallsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Halls
        [AllowAnonymous]
        public ActionResult Index()
        {
            var halls = db.Halls.Include(x=>x.Rows).Include(h => h.Cinema);
            return View(halls.ToList());
        }

        // GET: Halls/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hall hall = db.Halls.Find(id);
            if (hall == null)
            {
                return HttpNotFound();
            }
            return View(hall);
        }

        // GET: Halls/Create
        public ActionResult Create()
        {
            ViewBag.CinemaId = new SelectList(db.Cinemas, "Id", "Name");
            return View();
        }

        // POST: Halls/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,CinemaId")] Hall hall)
        {
            if (ModelState.IsValid)
            {
                db.Halls.Add(hall);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CinemaId = new SelectList(db.Cinemas, "Id", "Name", hall.CinemaId);
            return View(hall);
        }

        // GET: Halls/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hall hall = db.Halls.Find(id);
            if (hall == null)
            {
                return HttpNotFound();
            }
            ViewBag.CinemaId = new SelectList(db.Cinemas, "Id", "Name", hall.CinemaId);
            return View(hall);
        }

        // POST: Halls/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,CinemaId")] Hall hall)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hall).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CinemaId = new SelectList(db.Cinemas, "Id", "Name", hall.CinemaId);
            return View(hall);
        }

        // GET: Halls/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hall hall = db.Halls.Find(id);
            if (hall == null)
            {
                return HttpNotFound();
            }
            return View(hall);
        }

        // POST: Halls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Hall hall = db.Halls.Find(id);
            var rows = db.Rows.Where(x => x.HallId == hall.Id);
            if(rows != null)
            {
                foreach(var row in rows)
                {
                    db.Rows.Remove(row);
                }
            }
            db.Halls.Remove(hall);
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
