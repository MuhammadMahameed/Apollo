using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using Apollo.Data;
using Apollo.Models;

namespace Apollo.Services
{
    public class SongService
    {
        private readonly DataContext _context;
        public SongService(DataContext context)
        {
            _context = context;
        }

        public List<Song> GetMatchingSongs(string str)
        {
            var strToLower = str.ToLower();
            var matchingSongs = _context.Song.Where(s => s.Title.ToLower().Contains(strToLower)).ToList();
            return matchingSongs;
        }
    }
}
