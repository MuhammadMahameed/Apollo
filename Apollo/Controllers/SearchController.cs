using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apollo.Data;
using Microsoft.AspNetCore.Mvc;

namespace Apollo.Controllers
{
    public class SearchController : Controller
    {
        private readonly DataContext _context;

        public SearchController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData["artists"] = _context.Artist.ToList();
            ViewData["songs"] = _context.Song.ToList();
            ViewData["albums"] = _context.Album.ToList();
            return View();
        }
    }
}
