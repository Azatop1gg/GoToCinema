using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GoToCinema.Models.DomainModels
{
    public class Row
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Индекс ряда")]
        public string RowName { get; set; }

        [Required]
        [Display(Name = "Зал")]
        public int HallId { get; set; }

        [Display(Name = "Информация")]
        public string Note { get; set; }
        public Hall Hall { get; set; }
        public ICollection<Seat> Seats { get; set; }
    }
}