using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Apollo.Models;

namespace Apollo.Data
{
    public class SongContext : DbContext
    {
        public SongContext (DbContextOptions<SongContext> options)
            : base(options)
        {
        }

        public DbSet<Apollo.Models.Song> Song { get; set; }
    }
}
