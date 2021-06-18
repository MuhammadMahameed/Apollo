using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Apollo.Data;
using Apollo.Models;
using Apollo.Services;
using Newtonsoft.Json;

namespace Apollo.Controllers
{
    public class SongsController : Controller
    {
        private readonly DataContext _context;
        private readonly SongService _songService;

        public SongsController(DataContext context, SongService songService)
        {
            _context = context;
            _songService = songService;
        }

        // GET: Songs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Song.Include(x => x.Album).ToListAsync());
        }

        public IActionResult Search(string matchingStr)
        {
            return Json(_songService.GetMatchingSongs(matchingStr));
        }


        // GET: Songs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var song = await _context.Song
                .FirstOrDefaultAsync(m => m.Id == id);
            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }

        // GET: Songs/Create
        public IActionResult Create()
        {
            ViewData["categories"] = new SelectList(_context.Category, nameof(Category.Id), nameof(Category.Name));
            ViewData["artists"] = new SelectList(_context.Artist, nameof(Artist.Id), nameof(Artist.StageName));
            SelectList sl = new(_context.Album, nameof(Album.Id), nameof(Album.Title));
            IEnumerable<SelectListItem> enumerable = sl.Prepend(new SelectListItem("N/A", "0", true));
            ViewData["albums"] = enumerable;
            return View();
        }

        // POST: Songs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Plays,Rating,Length,ReleaseDate")] Song song, int Category, int Artist, int Album)
        {
            song.Plays = 0;
            song.Rating = 0;
            song.ReleaseDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                song.Category = _context.Category.FirstOrDefault(x => x.Id == Category);
                song.Artist = _context.Artist.FirstOrDefault(x => x.Id == Artist);
                song.Album = _context.Album.FirstOrDefault(x => x.Id == Album);

                _context.Add(song);
                await _context.SaveChangesAsync();

                if (song.Album != null)
                {
                    var changedAlbum = _context.Album.FirstOrDefault(x => x.Id == Album);
                    changedAlbum.ListenTime = changedAlbum.ListenTime.Add(song.Length);
                    _context.Update(changedAlbum);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(song);
        }

        // GET: Songs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var song = await _context.Song.FindAsync(id);
            if (song == null)
            {
                return NotFound();
            }
            return View(song);
        }

        // POST: Songs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Plays,Rating,Length,ReleaseDate")] Song song)
        {
            if (id != song.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(song);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SongExists(song.Id))
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
            return View(song);
        }

        // GET: Songs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var song = await _context.Song
                .FirstOrDefaultAsync(m => m.Id == id);
            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }

        // POST: Songs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var song = await _context.Song.FindAsync(id);
            _context.Song.Remove(song);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SongExists(int id)
        {
            return _context.Song.Any(e => e.Id == id);
        }
    }
}