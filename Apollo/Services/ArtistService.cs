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
    public class ArtistService
    {
        private readonly DataContext _context;
        public ArtistService(DataContext context)
        {
            _context = context;
        }

        public ArrayList GetMatchingArtists(string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return new ArrayList();
            }

            var strToLower = str.ToLower();
            var matchingArtists = _context.Artist.Where(a => a.StageName.ToLower().Contains(strToLower)).ToList();

            ArrayList matchingArtistsList = new ArrayList();

            foreach (Artist artist in matchingArtists)
            {
                matchingArtistsList.Add(new
                {
                    id = artist.Id,
                    stageName = artist.StageName,
                    image = artist.Image,
                    firstName = artist.FirstName,
                    lastName = artist.LastName,
                    rating = artist.Rating
                });
            }

            return matchingArtistsList;
        }

        public ArrayList FilterArtists(string str)
        {
            var strToLower = str.ToLower();

            var matchingArtists = _context.Artist
                .Include(s => s.Albums)
                .Include(s => s.Songs)
                .Where(s => s.FirstName.ToLower().Contains(strToLower) ||
                            s.LastName.ToLower().Contains(strToLower) ||
                            s.StageName.ToLower().Contains(strToLower) ||
                            s.Age.ToString().ToLower().Contains(strToLower) ||
                            s.Rating.ToString().ToLower().Contains(strToLower) ||
                            s.Image.ToLower().Contains(strToLower) ||
                            s.Songs.Any(x => x.Title.ToLower().Contains(strToLower)) ||
                            s.Albums.Any(x => x.Title.ToLower().Contains(strToLower)))
                .ToList();

            ArrayList matchingArtistsList = new ArrayList();

            foreach (Artist artist in matchingArtists)
            {
                matchingArtistsList.Add(new
                {
                    id = artist.Id,
                    firstName = artist.FirstName,
                    lastName = artist.LastName,
                    stageName = artist.StageName,
                    age = artist.Age,
                    rating = artist.Rating,
                    image = artist.Image,
                    songs = artist.Songs.Select(x => x.Title),
                    albums = artist.Albums.Select(x => x.Title)
                });
            }

            return matchingArtistsList;
        }
    }
}
