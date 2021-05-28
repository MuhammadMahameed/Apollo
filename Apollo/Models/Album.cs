using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apollo.Models
{
    public class Album
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public Artist[] Artists { get; set; }

        public Song[] Songs { get; set; }

        public TimeSpan ListenTime { get; set; }

        public int Plays { get; set; }

        public double Rating { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Cover { get; set; }

        public Category[] Category { get; set; }

    }
}
