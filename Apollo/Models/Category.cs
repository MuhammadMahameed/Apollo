using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Apollo.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Category name cannot be shorter than 2 or longer than 25 characters.")]
        public string Name { get; set; }

        public List<Song> Songs { get; set; }

        public List<Album> Albums { get; set; }

        public List<Artist> Artists { get; set; }
    }
}
