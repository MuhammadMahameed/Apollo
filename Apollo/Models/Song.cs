using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Apollo.Models
{
    public class Song
    {
        public int Id { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Title name cannot be shorter than 2 or longer than 25 characters.")]
        public string Title { get; set; }

        public Artist Artist { get; set; }

        public double Rating { get; set; }

        public Category Category { get; set; }

        [Required]
        public TimeSpan Length { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        public Album Album { get; set; }
    }
}
