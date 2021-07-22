using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apollo.Data;

namespace Apollo.Services
{
    public class BiographyService
    {
        private readonly DataContext _context;
        public BiographyService(DataContext context)
        {
            _context = context;
        }
    }
}
