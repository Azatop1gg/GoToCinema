using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using GoToCinema.Models.DomainModels.Enums;

namespace GoToCinema.Models.DomainModels
{
    public class SessionSeat
    {
        [Required]
        [Key]
        [Column(Order = 0)]
        public int RowId { get; set; }

        [Required]
        [Key]
        [Column(Order = 1)]
        public int SeatId { get; set; }

        [Required]
        [Key]
        [Column(Order = 2)]
        public int SessionId { get; set; }

        [Required]
        public SeatState SeatState { get; set; } = SeatState.Free;
        public int? UserId { get; set; }
        public virtual Seat Seat { get; set; }
        public virtual Session Session{ get; set; }
        public virtual Row Row { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}