using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GoToCinema.Models.DomainModels
{
    public class Session
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Начало сеанса")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Конец сеанса")]
        public DateTime EndTime { get; set; }

        [Required]
        [Display(Name = "Зал")]
        public int HallId { get; set; }

        [Required]
        [Display(Name = "Фильм")]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public Hall Hall { get; set; }
        public ICollection<SessionSeat> SessionSeats { get; set; }
    }
}