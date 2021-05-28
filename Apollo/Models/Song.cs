using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apollo.Models
{
    public class Song
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public Artist[] Artists { get; set; }

        public int Plays { get; set; }

        public double Rating { get; set; }

        public Category[] Category { get; set; }

        public TimeSpan Length { get; set; }

        public DateTime ReleaseDate { get; set; }

        public Album Album { get; set; }
    }
}
