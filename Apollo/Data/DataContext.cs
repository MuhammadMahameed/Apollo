﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Apollo.Models;

namespace Apollo.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext (DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Apollo.Models.Album> Album { get; set; }

        public DbSet<Apollo.Models.Artist> Artist { get; set; }

        public DbSet<Apollo.Models.Category> Category { get; set; }

        public DbSet<Apollo.Models.Song> Song { get; set; }
    }
}