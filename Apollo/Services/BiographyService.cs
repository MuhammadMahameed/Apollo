using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apollo.Data;
using Apollo.Models;
using Microsoft.EntityFrameworkCore;

namespace Apollo.Services
{
    public class BiographyService
    {
        private readonly DataContext _context;
        public BiographyService(DataContext context)
        {
            _context = context;
        }

        public ArrayList FilterBiographies(string str)
        {
            var strToLower = str.ToLower();

            var matchingBiographies = _context.Biography
                .Include(x => x.Artist)
                .Where(s => s.Artist.StageName.ToLower().Contains(strToLower) ||
                            s.EarlyLife.ToLower().Contains(strToLower) ||
                            s.Career.ToLower().Contains(strToLower) ||
                            s.Artistry.ToLower().Contains(strToLower) ||
                            s.PersonalLife.ToLower().Contains(strToLower) ||
                            s.NumberOfSongs.ToString().ToLower().Contains(strToLower) ||
                            s.NumberOfAlbums.ToString().ToLower().Contains(strToLower))
                .ToList();

            ArrayList matchingBiographiesList = new ArrayList();

            foreach (Biography biography in matchingBiographies)
            {
                matchingBiographiesList.Add(new
                {
                    id = biography.Id,
                    artistStageName = biography.Artist.StageName,
                    earlyLife = biography.EarlyLife,
                    career = biography.Career,
                    artistry = biography.Artistry,
                    personalLife = biography.PersonalLife,
                    numberOfSongs = biography.NumberOfSongs,
                    numberOfAlbums = biography.NumberOfAlbums
                });
            }

            return matchingBiographiesList;
        }
    }
}
