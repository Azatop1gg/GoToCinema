using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using GoToCinema.Models;
using GoToCinema.Models.DomainModels;
using GoToCinema.Models.DomainModels.Enums;

namespace GoToCinema.Controllers
{
    [Authorize(Roles = "admin")]
    public class CinemasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        public ActionResult Index()
        {
            var cinemas = db.Cinemas.Include(h => h.Halls);
            return View(cinemas);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Cinema cinema)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            db.Cinemas.Add(cinema);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            var cinema = db.Cinemas.Find(id);
            if (cinema == null)
                return HttpNotFound();
            return View(cinema);
        }

        [AllowAnonymous]
        public ActionResult GetSessionsByDate(int cinemaId, DateTime date)
        {
            var sessions = db.Sessions
                    .Include(x => x.Hall)
                    .Where(x => x.Hall.CinemaId == cinemaId && DbFunctions.TruncateTime(x.Date) == date.Date)
                    .OrderBy(x => x.Date).ToList();
            return PartialView(sessions);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetBookingPage(int sessionId)
        {
            var userId = int.Parse(GetUserId((ClaimsPrincipal)User));
            ViewBag.UserId = userId;
            var session = db.Sessions.Include(x=>x.Hall).FirstOrDefault(x=>x.Id == sessionId);
            var sessionSeatState = db.SessionSeats.Where(x => x.SessionId == sessionId).Select(x => x.SeatState).ToArray();
            session.Hall.Rows = db.Rows.Where(x => x.HallId == session.HallId).ToList();
            foreach(var row in session.Hall.Rows)
            {
                row.Seats = db.Seats.Where(x => x.RowId == row.Id).ToList();
            }
            var sessionSeats = session.Hall.Rows.SelectMany(x => x.Seats.Select(y => new SessionSeat
            {
                RowId = y.RowId,
                SeatId = y.Id,
                SessionId = sessionId,
            })).ToList();
            var sessionUserIds = db.SessionSeats.Where(x => x.SessionId == sessionId).Select(x => x.UserId).ToArray();

            foreach(var sessionSeat in sessionSeats)
            {
                sessionSeat.Seat = db.Seats.Find(sessionSeat.SeatId);
                sessionSeat.Row = db.Rows.Find(sessionSeat.RowId);
            }
            if(sessionSeatState != null)
            {
                for(int i = 0; i < sessionSeatState.Length; i++)
                {
                    sessionSeats.ToArray()[i].SeatState = sessionSeatState[i];
                    sessionSeats.ToArray()[i].UserId = sessionUserIds[i];
                }
            }
            if (sessionSeats != null)
            {
                foreach (var sessionSeat in sessionSeats)
                {
                    var _sessionSeat = db.SessionSeats.FirstOrDefault(x => x.RowId == sessionSeat.RowId && x.SeatId == sessionSeat.SeatId && x.SessionId == sessionSeat.SessionId);
                    if (_sessionSeat == null)
                    {
                        db.SessionSeats.Add(sessionSeat);
                        db.SaveChanges();
                    }
                }
            }
            var bookingModel = new BookingViewModel { SessionSeats = sessionSeats };
            return View(bookingModel);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult GetBookingPage(int[] seatIds, int sessionId, string action)
        {
            if (seatIds == null)
                return RedirectToAction("Index");

            var userId = int.Parse(GetUserId((ClaimsPrincipal)User));

            var sessionSeats = new List<SessionSeat>();
            for(int i = 0; i < seatIds.Length; i++)
            {
                var seatId = seatIds[i];
                var sessionSeat = db.SessionSeats.FirstOrDefault(x => x.SeatId == seatId);
                if (sessionSeat != null)
                {
                    sessionSeats.Add(sessionSeat);
                }
                    
            }
            foreach (var sessionseat in sessionSeats)
            {
                if (action == "Бронировать")
                {
                    sessionseat.SeatState = SeatState.Booked;
                }
                else
                    sessionseat.SeatState = SeatState.Bought;

                sessionseat.UserId = userId;

                db.Entry(sessionseat).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var cinema = db.Cinemas.Find(id);
            if (cinema == null)
                return HttpNotFound();

            return View(cinema);
        }

        [HttpPost]
        public ActionResult Edit(Cinema cinema)
        {
            if (!ModelState.IsValid)
                return View(cinema);

            db.Entry(cinema).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var cinema = db.Cinemas.Find(id);
            if (cinema == null)
                return HttpNotFound();

            return View(cinema);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var cinema = db.Cinemas.Find(id);
            var halls = db.Halls.Where(x => x.CinemaId == cinema.Id);
            if (halls != null)
            {
                foreach(var hall in halls)
                {
                    db.Halls.Remove(hall);
                }
            }
            db.Cinemas.Remove(cinema);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        
        private string GetUserId(ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return null;

            ClaimsPrincipal currentUser = user;

            return currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}