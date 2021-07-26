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

        public ArrayList GetAllAlbums()
        {
            var albums = _context.Album.ToList();
            ArrayList albumsList = new ArrayList();
            var albumsUsed = new ArrayList();

            foreach (Album album in albums)
            {
                if (!albumsUsed.Contains(album.Title))
                {
                    albumsUsed.Add(album.Title);
                    albumsList.Add(new
                    {
                        id = album.Id,
                        title = album.Title
                    });
                }
            }

            return albumsList;
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

        public ArrayList FilterAlbums(string str)
        {
            var strToLower = str.ToLower();

            var matchingAlbums = _context.Album
                .Include(s => s.Artist)
                .Include(s => s.Songs)
                .Include(s => s.Category)
                .Where(s => s.Title.ToLower().Contains(strToLower) ||
                            s.Category.Name.ToLower().Contains(strToLower) ||
                            s.Artist.StageName.ToLower().Contains(strToLower) ||
                            s.ListenTime.ToString().ToLower().Contains(strToLower) ||
                            s.Plays.ToString().ToLower().Contains(strToLower) ||
                            s.Rating.ToString().ToLower().Contains(strToLower) ||
                            s.ReleaseDate.ToString().ToLower().Contains(strToLower) ||
                            s.Cover.ToLower().Contains(strToLower) ||
                            s.Songs.Any(x => x.Title.ToLower().Contains(strToLower)))
                .ToList();

            ArrayList matchingAlbumsList = new ArrayList();

            foreach (Album album in matchingAlbums)
            {
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
                    cover = album.Cover,
                    songs = album.Songs.Select(x => x.Title)
                });
            }

            return matchingAlbumsList;
        }

        public ArrayList FilterAlbumsByCategoryAndArtist(int categoryId, int artistId)
        {
            ArrayList matchingAlbumsList = new ArrayList();

            var matchingAlbums = _context.Album.Include(x => x.Category)
                                               .Where(x => x.Category.Id == categoryId && x.Artist.Id == artistId)
                                               .ToList();

            foreach (Album album in matchingAlbums)
            {
                matchingAlbumsList.Add(new
                {
                    id = album.Id,
                    title = album.Title,
                });
            }

            return matchingAlbumsList;
        }
    }
}
