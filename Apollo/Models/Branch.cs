using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Apollo.Models
{
    public class Branch
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Address Name")]
        public string AddressName { get; set; }

        [Required]
        [RegularExpression(@"^\d+\.\d+,\d+\.\d+$", ErrorMessage = "Please enter a correct coordinate")]
        public string Coordinate { get; set; }
    }
}
