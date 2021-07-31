using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apollo.Models
{
    public class Biography
    {
        public int Id { get; set; }

        public int ArtistId { get; set; }

        public Artist Artist { get; set; }

        [Required]
        [Display(Name = "Early Life")]
        [StringLength(100000, MinimumLength = 50, ErrorMessage = "Early life description cannot be shorter than 50 characters and longer than 100,000 characters.")]
        public string EarlyLife { get; set; }

        [Required]
        [StringLength(100000, MinimumLength = 50, ErrorMessage = "Career description cannot be shorter than 50 characters and longer than 100,000 characters.")]
        public string Career { get; set; }

        [Required]
        [StringLength(100000, MinimumLength = 50, ErrorMessage = "Artistry description cannot be shorter than 50 characters and longer than 100,000 characters.")]
        public string Artistry { get; set; }

        [Required]
        [Display(Name = "Personal Life")]
        [StringLength(100000, MinimumLength = 50, ErrorMessage = "Personal life description cannot be shorter than 50 characters and longer than 100,000 characters.")]
        public string PersonalLife { get; set; }

        [Display(Name = "Number of Songs")]
        public int NumberOfSongs { get; set; }

        [Display(Name = "Number of Albums")]
        public int NumberOfAlbums { get; set; }
    }
}
