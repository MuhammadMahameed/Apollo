using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using Apollo.Data;
using Apollo.Models;

namespace Apollo.Services
{
    public class ArtistService
    {
        private readonly DataContext _context;
        public ArtistService(DataContext context)
        {
            _context = context;
        }

        public List<Artist> GetMatchingArtists(string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return new List<Artist>();
            }

            var strToLower = str.ToLower();
            var matchingArtists = _context.Artist.Where(a => a.StageName.ToLower().Contains(strToLower)).ToList();
            return matchingArtists;
        }
    }
}
