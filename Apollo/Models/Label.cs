using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Apollo.Models
{
    public class Label
    {
        public int Id { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Label name cannot be shorter than 2 or longer than 25 characters.")]
        public string Name { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime Founded { get; set; }

        public List<Artist> Artists { get; set; }
    }
}
