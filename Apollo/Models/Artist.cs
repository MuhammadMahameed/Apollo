using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apollo.Models
{
    public class Artist
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string StagetName { get; set; }

        public int Age { get; set; }

        public Album[] Albums { get; set; }

        public Song[] Songs { get; set; }

        public double Rating { get; set; }

        public string Image { get; set; }
    }
}
