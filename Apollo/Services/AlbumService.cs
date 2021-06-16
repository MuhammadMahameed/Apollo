using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using Apollo.Data;
using Apollo.Models;

namespace Apollo.Services
{
    public class AlbumService
    {
        private readonly DataContext _context;
        public AlbumService(DataContext context)
        {
            _context = context;
        }

        public List<Album> GetMatchingAlbums(string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return new List<Album>();
            }

            var strToLower = str.ToLower();
            var matchingAlbums = _context.Album.Where(a => a.Title.ToLower().Contains(strToLower)).ToList();
            return matchingAlbums;
        }
    }
}
