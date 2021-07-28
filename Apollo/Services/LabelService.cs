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
    }
}
