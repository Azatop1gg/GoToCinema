using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GoToCinema.Models.DomainModels
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Название фильма")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Жанр")]
        public string Genre { get; set; }

        [Required]
        [Display(Name = "Продюсер")]
        public string Producer { get; set; }

        [Required]
        [Display(Name = "Год выпуска")]
        public int Year { get; set; }

        [Required]
        [Display(Name = "Страна")]
        public string Country { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }
        public ICollection<Session> Sessions { get; set; }

    }
}