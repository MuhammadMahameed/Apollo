using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using Apollo.Data;
using Apollo.Models;
using Microsoft.EntityFrameworkCore;

namespace Apollo.Services
{
    public class AlbumService
    {
        private readonly DataContext _context;
        public AlbumService(DataContext context)
        {
            _context = context;
        }

        public ArrayList GetMatchingAlbums(string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return new ArrayList();
            }

            var strToLower = str.ToLower();
            var matchingAlbums = _context.Album.Where(a => a.Title.ToLower().Contains(strToLower))
                .Include(x => x.Artist)
                .Include(x => x.Category)
                .Include(x => x.Songs)
                .ToList();

            ArrayList matchingAlbumsList = new ArrayList();

            foreach (Album album in matchingAlbums)
            {
                var numSongs = 0;
                if (album.Songs != null)
                {
                    numSongs = album.Songs.Count;
                }

                matchingAlbumsList.Add(new
                {
                    id = album.Id,
                    title = album.Title,
                    category = album.Category.Name,
                    artist = album.Artist.StageName,
                    listenTime = album.ListenTime,
                    plays = album.Plays,
                    rating = album.Rating,
                    releaseDate = album.ReleaseDate,
                    numSongs = numSongs,
                    cover = album.Cover
                });
            }

            return matchingAlbumsList;
        }
    }
}
