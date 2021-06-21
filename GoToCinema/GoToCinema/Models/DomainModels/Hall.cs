using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GoToCinema.Models.DomainModels
{
    public class Hall
    {

        public int Id { get; set; }

        [Required]
        [Display(Name = "Название зала")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Кинотеатр зала")]
        public int CinemaId { get; set; }

        public Cinema Cinema { get; set; }
        public ICollection<Row> Rows { get; set; }
        public ICollection<Session> Sessions { get; set; }

    }
}