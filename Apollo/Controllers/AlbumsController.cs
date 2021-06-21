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

namespace Apollo.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly DataContext _context;
        private readonly AlbumService _albumService;

        public AlbumsController(DataContext context, AlbumService albumService)
        {
            _context = context;
            _albumService = albumService;
        }

        // GET: Albums
        public async Task<IActionResult> Index()
        {
            return View(await _context.Album.Include(x => x.Songs)
                                            .Include(x => x.Artist)
                                            .Include(x => x.Category)
                                            .ToListAsync());
        }

        public IActionResult Search(string matchingStr)
        {
            return Json(_albumService.GetMatchingAlbums(matchingStr));
        }

        // GET: Albums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Album
                .FirstOrDefaultAsync(m => m.Id == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // GET: Albums/Create
        public IActionResult Create()
        {
            ViewData["songs"] = new MultiSelectList(_context.Song, nameof(Models.Song.Id), nameof(Models.Song.Title));
            ViewData["categories"] = new SelectList(_context.Category, nameof(Category.Id), nameof(Category.Name));
            ViewData["artists"] = new SelectList(_context.Artist, nameof(Artist.Id), nameof(Artist.StageName));
            return View();
        }

        // POST: Albums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ListenTime,Plays,Rating,ReleaseDate,Cover")] Album album, int Category, int Artist, int[] Songs)
        {
            if (_context.Album.Include(x => x.Artist).Any(x => x.Artist.Id == Artist && x.Title == album.Title))
            {
                var artist = _context.Artist.FirstOrDefault(x => x.Id == Artist).StageName;
                ModelState.AddModelError("Title", artist + " already has an album named " + "'" + album.Title + "'");
            }

            if (ModelState.IsValid)
            {
                // change listen time of old album
                foreach (int songId in Songs)
                {
                    var song = _context.Song.Include(x => x.Album).FirstOrDefault(x => x.Id == songId);
                    song.Album.ListenTime = new TimeSpan(0, 0, 0);

                    foreach (Song songRecord in song.Album.Songs.Where(x => x.Id != song.Id))
                    {
                        song.Album.ListenTime = song.Album.ListenTime.Add(songRecord.Length);
                    }

                    // update old album
                    _context.Update(song.Album);
                    await _context.SaveChangesAsync();
                }

                album.ListenTime = new TimeSpan(0, 0, 0);
                album.Plays = 0;
                album.Rating = 0;
                album.ReleaseDate = DateTime.Now;
                album.Category = _context.Category.FirstOrDefault(x => x.Id == Category);
                album.Artist = _context.Artist.FirstOrDefault(x => x.Id == Artist);
                album.Songs = _context.Song.Where(x => Songs.Contains(x.Id)).ToList();

                foreach(Song song in album.Songs)
                {
                    album.ListenTime = album.ListenTime.Add(song.Length);
                }

                _context.Add(album);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(album);
        }

        public IActionResult Filter(string matchingStr)
        {
            return Json(_albumService.FilterAlbums(matchingStr));
        }

        public IActionResult FilterAlbumsByCategoryAndArtist(int categoryId, int artistId)
        {
            return Json(_albumService.FilterAlbumsByCategoryAndArtist(categoryId, artistId));
        }

        // GET: Albums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Album.FindAsync(id);

            if (album == null)
            {
                return NotFound();
            }

            album = _context.Album.Include(x => x.Songs).FirstOrDefault(x => x.Id == album.Id);

            SelectList slArtists, slCategories;
            IEnumerable<SelectListItem> enumerableArtist, enumerableCategory;

            List<Artist> artists = _context.Artist.ToList();
            Artist artist = artists.FirstOrDefault(x => x.Id == album.Artist.Id);
            artists.Remove(artist);
            slArtists = new(artists, nameof(Artist.Id), nameof(Artist.StageName));
            enumerableArtist = slArtists.Prepend(new SelectListItem(artist.StageName, artist.Id.ToString(), true));

            List<Category> categories = _context.Category.ToList();
            Category category = categories.FirstOrDefault(x => x.Id == album.Category.Id);
            categories.Remove(category);
            slCategories = new(categories, nameof(Category.Id), nameof(Category.Name));
            enumerableCategory = slCategories.Prepend(new SelectListItem(category.Name, category.Id.ToString(), true));

            ViewData["songs"] = new MultiSelectList(_context.Song, nameof(Song.Id), nameof(Song.Title));
            ViewData["selectedSongs"] = album.Songs.Select(x => x.Id).ToList();
            ViewData["artists"] = enumerableArtist;
            ViewData["categories"] = enumerableCategory;
            return View(album);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ListenTime,Plays,Rating,ReleaseDate,Cover")] Album album, int Artist, int Category, int[] Songs)
        {
            if (id != album.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // change listen time of old album
                    foreach (int songId in Songs)
                    {
                        var song = _context.Song.Include(x => x.Album).FirstOrDefault(x => x.Id == songId);
                        if (song.Album != null)
                        {
                            song.Album.ListenTime = new TimeSpan(0, 0, 0);

                            foreach (Song songRecord in song.Album.Songs.Where(x => x.Id != song.Id))
                            {
                                song.Album.ListenTime = song.Album.ListenTime.Add(songRecord.Length);
                            }

                            // update old album
                            _context.Update(song.Album);
                            await _context.SaveChangesAsync();
                        }
                    }

                    album = _context.Album.Include(x => x.Songs)
                                          .Include(x => x.Artist)
                                          .Include(x => x.Category)
                                          .FirstOrDefault(x => x.Id == album.Id);

                    album.Songs = _context.Song.Where(x => Songs.Contains(x.Id)).ToList();
                    album.Artist = _context.Artist.FirstOrDefault(x => x.Id == Artist);
                    album.Category = _context.Category.FirstOrDefault(x => x.Id == Category);
                    album.ListenTime = new TimeSpan(0, 0, 0);

                    // update listentime
                    foreach (Song songRecord in album.Songs)
                    {
                       album.ListenTime = album.ListenTime.Add(songRecord.Length);
                    }

                    _context.Update(album);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumExists(album.Id))
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
            return View(album);
        }

        // GET: Albums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Album
                .FirstOrDefaultAsync(m => m.Id == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var album = _context.Album.Include(x => x.Songs).FirstOrDefault(x => x.Id == id);
            album.Songs = null;
            _context.Album.Update(album);
            _context.Album.Remove(album);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlbumExists(int id)
        {
            return _context.Album.Any(e => e.Id == id);
        }
    }
}
