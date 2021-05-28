using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Apollo.Models;

namespace Apollo.Data
{
    public class ArtistContext : DbContext
    {
        public ArtistContext (DbContextOptions<ArtistContext> options)
            : base(options)
        {
        }

        public DbSet<Apollo.Models.Artist> Artist { get; set; }
    }
}
