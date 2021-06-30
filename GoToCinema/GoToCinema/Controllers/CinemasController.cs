using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GoToCinema.Models;
using GoToCinema.Models.DomainModels;
using GoToCinema.Models.DomainModels.Enums;
using Microsoft.AspNet.Identity.Owin;

namespace GoToCinema.Controllers
{
    [Authorize(Roles = "admin")]
    public class CinemasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
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
        public ActionResult Create(Cinema cinema, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid && uploadImage != null)
            {
                byte[] imageData = null;

                using(var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }

                cinema.Image = imageData;

                db.Cinemas.Add(cinema);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();

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
                    .Where(x => x.Hall.CinemaId == cinemaId && DbFunctions.TruncateTime(x.Date) == date.Date && x.Date > DateTime.Now)
                    .OrderBy(x => x.Date)
                    .Include(x=>x.Movie)
                    .ToList();

            return PartialView(sessions);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetBookingPage(int sessionId)
        {
            var userId = int.Parse(GetUserId((ClaimsPrincipal)User));
            ViewBag.UserId = userId;
            var session = db.Sessions
                        .Include(x=>x.Hall)
                        .Include(x=>x.Movie)
                        .Include(x=>x.Hall.Rows)
                        .Include(x=>x.Hall.Rows.Select(y=>y.Seats))
                        .FirstOrDefault(x=>x.Id == sessionId);
            var sessionSeats = db.SessionSeats.Where(x => x.SessionId == sessionId).ToList();

            var sessionSeatsView = session.Hall.Rows.SelectMany(x => x.Seats.Select(y =>
            {
                var sessionSeat = sessionSeats.FirstOrDefault(z => z.RowId == y.RowId && z.SeatId == y.Id && z.SessionId == sessionId);
                return new SessionSeat
                {
                    RowId = y.RowId,
                    Row = x,
                    SeatId = y.Id,
                    SessionId = sessionId,
                    SeatState = sessionSeat?.SeatState ?? SeatState.Free,
                    UserId = sessionSeat?.UserId ?? null
                };
            })).ToList();

            for (int i = 0; i < sessionSeatsView.Count(); i++)
            {
                var seatId = sessionSeatsView[i].SeatId;
                var sessionSeat = db.SessionSeats.FirstOrDefault(x => x.SessionId == sessionId && x.SeatId == seatId);
                
                if(sessionSeat == null)
                {
                    sessionSeat = sessionSeatsView[i];
                    db.SessionSeats.Add(sessionSeat);
                    db.SaveChanges();
                }
            }

            var bookingModel = new BookingViewModel { SessionSeats = sessionSeatsView };

            ViewBag.Movie = session.Movie.Name;

            return PartialView(bookingModel);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> GetBookingPage(int[] freeSeatIds, int[] bookedSeatIds, int sessionId, SeatAction action)
        {
            int userId = int.Parse(GetUserId((ClaimsPrincipal)User));
            var user = db.Users.Find(userId);
            if(bookedSeatIds != null)
            {
                for(int i = 0; i < bookedSeatIds.Length; i++)
                {
                    var seatId = bookedSeatIds[i];
                    var _sessionSeat = db.SessionSeats.FirstOrDefault(x=>x.SeatId == seatId && x.SessionId == sessionId);
                    if (_sessionSeat != null)
                    {
                        db.SessionSeats.Remove(_sessionSeat);
                        db.SaveChanges();
                    }
                }
            }

            var userSeats = db.SessionSeats.Where(x => x.UserId == userId && x.SessionId == sessionId).Count();
            if (freeSeatIds == null)
                return RedirectToAction("Index");

            if (freeSeatIds.Count()+userSeats > 6)
            {
                var sessionSeats = db.SessionSeats.Where(x => x.SessionId == sessionId).ToList();
                ModelState.AddModelError("", "Купить/Забронировать разрешается не больше шести мест!");
                var bookingViewModel = new BookingViewModel() { SessionSeats = sessionSeats };
                return View();
            }
            
            string text = "";
            if(action == SeatAction.Book)
            {
                for(int i = 0; i < freeSeatIds.Length; i++)
                {
                    int seatId = freeSeatIds[i];
                    var _sessionSeat = db.SessionSeats.Include(x=>x.Row).FirstOrDefault(x => x.SeatId == seatId && x.SessionId == sessionId);
                    _sessionSeat.SeatState = SeatState.Booked;
                    _sessionSeat.UserId = userId;
                    db.Entry(_sessionSeat).State = EntityState.Modified;
                    text = text + $"{_sessionSeat.Row.RowName}{_sessionSeat.Seat.SeatNumber},";
                }
                await UserManager.SendEmailAsync(userId, "GoToCinema", $"Здравствуйте, {user.UserName}. Вы успешно забронировали места {text}");
            }

            if (action == SeatAction.Buy)
            {
                for (int i = 0; i < freeSeatIds.Length; i++)
                {
                    int seatId = freeSeatIds[i];
                    var _sessionSeat = db.SessionSeats.FirstOrDefault(x => x.SeatId == seatId && x.SessionId == sessionId);
                    _sessionSeat.SeatState = SeatState.Bought;
                    _sessionSeat.UserId = userId;
                    db.Entry(_sessionSeat).State = EntityState.Modified;
                    text = text + $"{_sessionSeat.Row.RowName}{_sessionSeat.Seat.SeatNumber},";
                }
                await UserManager.SendEmailAsync(userId, "GoToCinema", $"Здравствуйте, {user.UserName}. Вы успешно купили места {text}");
            }
            db.SaveChanges();

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
        public ActionResult Edit(Cinema cinema, HttpPostedFileBase uploadImage)
        {
            if (!ModelState.IsValid)
                return View(cinema);
            
            if (uploadImage == null)
            {
                ModelState.AddModelError("", "Загрузите изображение");
                return View(cinema);
            }

            byte[] imageData = null;

            using (var binaryReader = new BinaryReader(uploadImage.InputStream))
            {
                imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
            }
            cinema.Image = imageData;
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
            var halls = db.Halls.Where(x => x.CinemaId == cinema.Id).ToList();
            if (halls != null)
            {
                foreach(var hall in halls)
                {
                    var rows = db.Rows.Where(x => x.HallId == hall.Id).ToList();
                    var sessions = db.Sessions.Where(x => x.HallId == hall.Id).ToList();
                    if(sessions != null)
                    {
                        db.Sessions.RemoveRange(sessions);
                    }
                    if (rows != null)
                    {
                        foreach(Row row in rows)
                        {
                            var seats = db.Seats.Where(x => x.RowId == row.Id).ToList();
                            if(seats != null)
                            {
                                foreach(var seat in seats)
                                {
                                    var sessionSeats = db.SessionSeats.Where(x => x.SeatId == seat.Id).ToList();
                                    if (sessionSeats != null)
                                        db.SessionSeats.RemoveRange(sessionSeats);
                                }
                                
                                db.Seats.RemoveRange(seats);
                            }
                        }

                        db.Rows.RemoveRange(rows);
                    }

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