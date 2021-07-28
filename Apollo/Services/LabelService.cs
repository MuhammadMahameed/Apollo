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
    public class LabelService
    {
        private readonly DataContext _context;
        public LabelService(DataContext context)
        {
            _context = context;
        }

        public ArrayList FilterLabelsByArtistId(int artistId)
        {
            ArrayList matchingLabelsList = new ArrayList();

            var matchingLabels = _context.Artist.Include(x => x.Labels).FirstOrDefault(x => x.Id == artistId).Labels.ToList();

            foreach (Label label in matchingLabels)
            {
                matchingLabelsList.Add(new
                {
                    id = label.Id,
                    name = label.Name,
                });
            }

            return matchingLabelsList;
        }
    }
}
