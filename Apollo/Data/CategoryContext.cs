using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Apollo.Models;

namespace Apollo.Data
{
    public class CategoryContext : DbContext
    {
        public CategoryContext (DbContextOptions<CategoryContext> options)
            : base(options)
        {
        }

        public DbSet<Apollo.Models.Category> Category { get; set; }
    }
}
