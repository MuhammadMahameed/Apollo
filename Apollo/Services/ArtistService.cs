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
            var matchingArtists = _context.Artist.Where(a => a.StageName.ToLower().Contains(strToLower))
                .Include(x => x.Category)
                .ToList();

            ArrayList matchingArtistsList = new ArrayList();

            foreach (Artist artist in matchingArtists)
            {
                matchingArtistsList.Add(new
                {
                    id = artist.Id,
                    stageName = artist.StageName,
                    category = artist.Category.Name,
                    image = artist.Image,
                    firstName = artist.FirstName,
                    lastName = artist.LastName,
                    rating = artist.Rating
                });
            }

            return matchingArtistsList;
        }
    }
}
