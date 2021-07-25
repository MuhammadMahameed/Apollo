using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Apollo.Data;
using Apollo.Models;

namespace Apollo.Controllers
{
    public class BiographiesController : Controller
    {
        private readonly DataContext _context;

        public BiographiesController(DataContext context)
        {
            _context = context;
        }

        // GET: Biographies
        public async Task<IActionResult> Index()
        {
            var songs_gb = _context.Song.Include(x => x.Artist)
                                        .GroupBy(x => x.Artist.StageName)
                                        .Select(x => new
                                        {
                                            artistStageName = x.Key,
                                            count = x.Count()
                                        }).ToList();
            var albums_gb = _context.Album.Include(x => x.Artist)
                                            .GroupBy(x => x.Artist.StageName)
                                            .Select(x => new
                                            {
                                                artistStageName = x.Key,
                                                count = x.Count()
                                            }).ToList();

            var biographies = _context.Biography.Include(x => x.Artist).ToList();
            biographies.ForEach(biography =>
            {
                var temp = songs_gb.FirstOrDefault(s => s.artistStageName == biography.Artist.StageName);
                if (temp != null)
                    biography.NumberOfSongs = temp.count;

                temp = albums_gb.FirstOrDefault(s => s.artistStageName == biography.Artist.StageName);
                if (temp != null)
                    biography.NumberOfAlbums = temp.count;

                _context.Update(biography);
            });

            await _context.SaveChangesAsync();
            var dataContext = _context.Biography.Include(b => b.Artist);
            return View(await dataContext.ToListAsync());
        }

        // GET: Biographies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var biography = await _context.Biography
                .Include(b => b.Artist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (biography == null)
            {
                return NotFound();
            }

            return View(biography);
        }

        // GET: Biographies/Create
        public IActionResult Create()
        {
            ViewData["ArtistId"] = new SelectList(_context.Artist, "Id", "StageName");
            return View();
        }

        // POST: Biographies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ArtistId,EarlyLife,Career,Artistry,PersonalLife,NumberOfSongs,NumberOfAlbums")] Biography biography)
        {
            var exists = _context.Biography.FirstOrDefault(x => x.ArtistId == biography.ArtistId);

            if (exists != null)
            {
                ModelState.AddModelError("ArtistId", "This artist already has a biography");
            }

            if (ModelState.IsValid)
            {
                _context.Add(biography);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistId"] = new SelectList(_context.Artist, "Id", "StageName", biography.ArtistId);
            return View(biography);
        }

        // GET: Biographies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var biography = await _context.Biography.FindAsync(id);
            if (biography == null)
            {
                return NotFound();
            }
            ViewData["ArtistId"] = new SelectList(_context.Artist, "Id", "StageName", biography.ArtistId);
            return View(biography);
        }

        // POST: Biographies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ArtistId,EarlyLife,Career,Artistry,PersonalLife,NumberOfSongs,NumberOfAlbums")] Biography biography)
        {
            if (id != biography.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(biography);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BiographyExists(biography.Id))
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
            ViewData["ArtistId"] = new SelectList(_context.Artist, "Id", "StageName", biography.ArtistId);
            return View(biography);
        }

        // GET: Biographies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var biography = await _context.Biography
                .Include(b => b.Artist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (biography == null)
            {
                return NotFound();
            }

            return View(biography);
        }

        // POST: Biographies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var biography = await _context.Biography.FindAsync(id);
            _context.Biography.Remove(biography);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BiographyExists(int id)
        {
            return _context.Biography.Any(e => e.Id == id);
        }
    }
}
