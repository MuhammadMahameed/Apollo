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

        [Display(Name = "Early Life")]
        public string EarlyLife { get; set; }

        public string Career { get; set; }

        public string Artistry { get; set; }

        [Display(Name = "Personal Life")]
        public string PersonalLife { get; set; }
    }
}
