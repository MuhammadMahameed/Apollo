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

        public ArrayList GetAllLabels()
        {
            ArrayList labels = new ArrayList();

            foreach (Label label in _context.Label.ToList())
            {
                labels.Add(new
                {
                    id = label.Id,
                    name = label.Name,
                });
            }

            return labels;
        }

        public ArrayList FilterLabels(string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return new ArrayList();
            }

            var strToLower = str.ToLower();

            var matchingLabels = _context.Label
                .Include(x => x.Artists)
                .Where(s => s.Name.ToLower().Contains(str) ||
                            s.Status.ToLower().Contains(str) ||
                            s.Country.ToLower().Contains(str) ||
                            s.Founded.ToString().ToLower().Contains(str) ||
                            s.Artists.Any(x => x.StageName.ToLower().Contains(strToLower)))
                .ToList();

            ArrayList matchingLabelsList = new ArrayList();

            foreach (Label label in matchingLabels)
            {
                matchingLabelsList.Add(new
                {
                    id = label.Id,
                    name = label.Name,
                    status = label.Status,
                    country = label.Country,
                    founded = label.Founded,
                    artists = label.Artists.Select(x => x.StageName)
                });
            }

            return matchingLabelsList;
        }
    }
}
