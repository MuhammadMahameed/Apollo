﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Apollo.Models
{
    public class Album
    {
        public int Id { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Title name cannot be shorter than 2 or longer than 25 characters.")]
        public string Title { get; set; }

        public Artist Artist { get; set; }

        public List<Song> Songs { get; set; }

        public TimeSpan ListenTime { get; set; }

        public int Plays { get; set; }

        public double Rating { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [RegularExpression("^.+\\.(png|jpg)$")]
        public string Cover { get; set; }

        public Category Category { get; set; }

    }
}
