using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using GoToCinema.Models.DomainModels.Enums;

namespace GoToCinema.Models.DomainModels
{
    public class Seat
    {
        public int Id { get; set; }

        [Required]
        [Display(Name ="Номер места")]
        public int SeatNumber { get; set; }

        [Required]
        [Display(Name = "Ряд")]
        public int RowId { get; set; }
        public Row Row { get; set; }
    }
}