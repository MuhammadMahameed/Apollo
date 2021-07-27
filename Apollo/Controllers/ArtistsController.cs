using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Apollo.Data;
using Apollo.Models;
using Microsoft.EntityFrameworkCore;
using Apollo.Services;
using System;
using Microsoft.AspNetCore.Authorization;

namespace Apollo.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly DataContext _context;
        private readonly ArtistService _artistService;

        public ArtistsController(DataContext context, ArtistService artistService)
        {
            _context = context;
            _artistService = artistService;
        }

        public IActionResult GetAllArtists()
        {
            return Json(_artistService.GetAllArtists());
        }

        public IActionResult Search(string matchingStr)
        {
            return Json(_artistService.GetMatchingArtists(matchingStr));
        }

        public IActionResult Filter(string matchingStr)
        {
            return Json(_artistService.FilterArtists(matchingStr));
        }

        // GET: Artists
        public async Task<IActionResult> Index()
        {
            return View(await _context.Artist
                .Include(x => x.Albums)
                .Include(x => x.Songs)
                .ToListAsync());
        }

        // GET: Artists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _context.Artist
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        // GET: Artists/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Artists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,StageName,Age,Rating,Image")] Artist artist)
        {
            var artist_with_this_stage_name = _context.Artist.FirstOrDefault(x => x.StageName.ToUpper().Equals(artist.StageName.ToUpper()));

            if (artist_with_this_stage_name != null)
            {
                ModelState.AddModelError("StageName", "This stage name is already used");
            }

            artist.Rating = 0;

            if (ModelState.IsValid)
            {
                _context.Add(artist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(artist);
        }

        // GET: Artists/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _context.Artist.FindAsync(id);
            if (artist == null)
            {
                return NotFound();
            }
            return View(artist);
        }

        // POST: Artists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,StageName,Age,Rating,Image")] Artist artist)
        {
            if (id != artist.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(artist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistExists(artist.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(artist);
        }

        // GET: Artists/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _context.Artist
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        // POST: Artists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var artist = _context.Artist.Include(x => x.Albums)
                                        .Include(x => x.Songs)
                                        .FirstOrDefault(x => x.Id == id);

            foreach(Song song in artist.Songs)
            {
                _context.Song.Remove(song);
            }

            foreach (Album album in artist.Albums)
            {
                _context.Album.Remove(album);
            }


            artist.Songs = null;
            artist.Albums = null;
            _context.Artist.Update(artist);
            _context.Artist.Remove(artist);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtistExists(int id)
        {
            return _context.Artist.Any(e => e.Id == id);
        }
    }
}
