using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Apollo.Data;
using Apollo.Models;
using FluentValidation;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Apollo.Models
{
    public class Artist
    {
        public int Id { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Stage name cannot be shorter than 2 or longer than 25 characters.")]
        [Display(Name = "Stage Name")]
        public string StageName { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Not a first name")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "First name cannot be shorter than 2 or longer than 25 characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Not a last name")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Last name cannot be shorter than 2 or longer than 25 characters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Range(10,130)]
        public int Age { get; set; }

        public List<Album> Albums { get; set; }

        public List<Song> Songs { get; set; }

        public double Rating { get; set; }

        [Required]
        [RegularExpression("^.+\\.(png|jpg)$", ErrorMessage = "Not an image url")]
        [Display(Name = "Image Url")]
        public string Image { get; set; }

        public Biography Biography { get; set; }

        public List<Label> Labels { get; set; }
    }
}