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
        [RegularExpression(@"^[-+]?([1-8]?\d(\.\d+)?|90(\.0+)?),\s*[-+]?(180(\.0+)?|((1[0-7]\d)|([1-9]?\d))(\.\d+)?)$", ErrorMessage = "Please enter a correct coordinate with a valid range")]
        public string Coordinate { get; set; }
    }
}
