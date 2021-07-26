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
    public class CategoryService
    {
        private readonly DataContext _context;
        public CategoryService(DataContext context)
        {
            _context = context;
        }

        public ArrayList GetAllCategories()
        {
            var categories = _context.Category.ToList();
            ArrayList categoriesList = new ArrayList();

            foreach (Category category in categories)
            {
                categoriesList.Add(new
                {
                    id = category.Id,
                    name = category.Name
                });
            }

            return categoriesList;
        }

        public ArrayList FilterCategories(string str)
        {
            var strToLower = str.ToLower();

            var matchingCategories = _context.Category
                .Where(s => s.Name.ToLower().Contains(strToLower))
                .ToList();

            ArrayList matchingCategoriesList = new ArrayList();

            foreach (Category category in matchingCategories)
            {
                matchingCategoriesList.Add(new
                {
                    id = category.Id,
                    title = category.Name
                });
            }

            return matchingCategoriesList;
        }
    }
}
