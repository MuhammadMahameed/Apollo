using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Apollo.Models
{
    public class Vote
    {
        public int Id { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public int RecordId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public int Score { get; set; }
    }
}
