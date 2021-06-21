using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoToCinema.Models.DomainModels
{
    public class BookingViewModel
    {
        public ICollection<SessionSeat> SessionSeats { get; set; }
    }
}