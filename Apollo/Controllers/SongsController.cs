﻿using System;
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
using Microsoft.AspNetCore.Authorization;

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
            return View(await _context.Song.Include(x => x.Album)
                                           .Include(x => x.Artist)
                                           .Include(x => x.Category)
                                           .ToListAsync());
        }

        public IActionResult Search(string matchingStr)
        {
            return Json(_songService.GetMatchingSongs(matchingStr));
        }

        public IActionResult Filter(string matchingStr)
        {
            return Json(_songService.FilterSongs(matchingStr));
        }

        public IActionResult FilterSongsByCategoryAndArtist(int categoryId, int artistId)
        {
            return Json(_songService.FilterSongsByCategoryAndArtist(categoryId, artistId));
        }

        public IActionResult GetNumberOfSongsPerCategory()
        {
            return Json(_songService.GetNumberOfSongsPerCategory());
        }

        public IActionResult GetArtistsPerCategoryHeatmapData()
        {
            return Json(_songService.GetArtistsPerCategoryHeatmapData());
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
        [Authorize(Roles = "Admin")]
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
            
            Artist artist = _context.Artist.Include(x => x.Songs).FirstOrDefault(x => x.Id == Artist);

            // The same artist can't have the same song title for more than 1 song
            if(artist.Songs.Select(x => x.Title).Contains(song.Title))
            {
                ModelState.AddModelError("Title", artist.StageName + " already has a song named " + song.Title);
            }

            // Songs have to be atleast 1 minute long
            if(song.Length.TotalSeconds < 60)
            {
                ModelState.AddModelError("Length", song.Title + " has to be atleast 1 minute long");
            }

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

            ViewData["categories"] = new SelectList(_context.Category, nameof(Models.Category.Id), nameof(Models.Category.Name));
            ViewData["artists"] = new SelectList(_context.Artist, nameof(Models.Artist.Id), nameof(Models.Artist.StageName));
            SelectList sl = new(_context.Album, nameof(Models.Album.Id), nameof(Models.Album.Title));
            IEnumerable<SelectListItem> enumerable = sl.Prepend(new SelectListItem("N/A", "0", true));
            ViewData["albums"] = enumerable;
            return View(song);
        }

        // GET: Songs/Edit/5
        [Authorize(Roles = "Admin")]
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

            song = _context.Song.Include(x => x.Album)
                                .Include(x => x.Artist)
                                .Include(x => x.Category)
                                .FirstOrDefault(x => x.Id == song.Id);

            SelectList slAlbums;
            SelectList slArtists;
            SelectList slCategories;
            IEnumerable<SelectListItem> enumerableAlbum, enumerableArtist, enumerableCategory;

            if (song.Album != null)
            {
                List<Album> albums = _context.Album.ToList();
                Album album = albums.FirstOrDefault(x => x.Id == song.Album.Id);
                albums.Remove(album);
                slAlbums = new(albums, nameof(Album.Id), nameof(Album.Title));
                enumerableAlbum = slAlbums.Append(new SelectListItem("N/A", "0", false));
                enumerableAlbum = enumerableAlbum.Prepend(new SelectListItem(album.Title, album.Id.ToString(), true));
            }
            else
            {
                slAlbums = new(_context.Album, nameof(Album.Id), nameof(Album.Title));
                enumerableAlbum = slAlbums.Append(new SelectListItem("N/A", "0", true));
            }

            List<Artist> artists = _context.Artist.ToList();
            Artist artist = artists.FirstOrDefault(x => x.Id == song.Artist.Id);
            artists.Remove(artist);
            slArtists = new(artists, nameof(Artist.Id), nameof(Artist.StageName));
            enumerableArtist = slArtists.Prepend(new SelectListItem(artist.StageName, artist.Id.ToString(), true));

            List<Category> categories = _context.Category.ToList();
            Category category = categories.FirstOrDefault(x => x.Id == song.Category.Id);
            categories.Remove(category);
            slCategories = new(categories, nameof(Category.Id), nameof(Category.Name));
            enumerableCategory = slCategories.Prepend(new SelectListItem(category.Name, category.Id.ToString(), true));

            ViewData["albums"] = enumerableAlbum;
            ViewData["artists"] = enumerableArtist;
            ViewData["categories"] = enumerableCategory;
            return View(song);
        }

        // POST: Songs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Plays,Rating,Length,ReleaseDate")] Song song, int Album, int Artist, int Category)
        {
            if (id != song.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var length = song.Length;
                    song = _context.Song.Include(x => x.Album)
                                            .Include(x => x.Artist)
                                            .Include(x => x.Category)
                                            .FirstOrDefault(x => x.Id == song.Id);

                    // if the song had a previous album and the id of the album changed,
                    // update the previous album's listentime
                    if (song.Album != null && song.Album.Id != Album)
                    {
                        song.Album.ListenTime = new TimeSpan(0, 0, 0);

                        // listen time of old album
                        foreach (Song songRecord in song.Album.Songs.Where(x => x.Id != song.Id))
                        {
                            song.Album.ListenTime = song.Album.ListenTime.Add(songRecord.Length);
                        }

                        // update old album
                        _context.Update(song.Album);
                    }

                    song.Length = length;
                    song.Album = _context.Album.FirstOrDefault(x => x.Id == Album);
                    song.Artist = _context.Artist.FirstOrDefault(x => x.Id == Artist);
                    song.Category = _context.Category.FirstOrDefault(x => x.Id == Category);
                    _context.Update(song);
                    await _context.SaveChangesAsync();

                    // update the length of the new album
                    Album album = _context.Album.Include(x => x.Songs).FirstOrDefault(x => x.Songs.Select(x => x.Title).Contains(song.Title));

                    if (album != null)
                    {
                        album.ListenTime = new TimeSpan(0, 0, 0);

                        // listen time of new album
                        foreach (Song songRecord in album.Songs)
                        {
                            album.ListenTime = album.ListenTime.Add(songRecord.Length);
                        }

                        // update new album
                        _context.Update(album);
                        await _context.SaveChangesAsync();
                    }
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
        [Authorize(Roles = "Admin")]
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
            var song = _context.Song.Include(x => x.Album).FirstOrDefault(x => x.Id == id);

            // remove the song from the album and entirly, and update the album's listen time
            if (song.Album != null)
            {
                song.Album.Songs.Remove(song);
                song.Album.ListenTime -= song.Length;
                _context.Album.Update(song.Album);
            }

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