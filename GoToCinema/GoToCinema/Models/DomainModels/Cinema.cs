using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GoToCinema.Models.DomainModels
{
    public class Cinema
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Название кинотеатра")]
        public string Name { get; set; }

        [Display(Name = "Изображение")]
        public byte[] Image { get; set; }

        [Display(Name = "Залы кинотеатра")]
        public ICollection<Hall> Halls { get; set; }
    }
}