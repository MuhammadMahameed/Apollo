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
using Microsoft.AspNetCore.Authorization;

namespace Apollo.Controllers
{
    public class LabelsController : Controller
    {
        private readonly DataContext _context;
        private readonly LabelService _labelService;

        public LabelsController(DataContext context, LabelService labelService)
        {
            _context = context;
            _labelService = labelService;
        }

        public IActionResult GetAllLabels()
        {
            return Json(_labelService.GetAllLabels());
        }

        public IActionResult Filter(string matchingStr)
        {
            return Json(_labelService.FilterLabels(matchingStr));
        }

        // GET: Labels
        [Authorize(Roles = "Admin,Client")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Label.Include(x => x.Artists).ToListAsync());
        }

        // GET: Labels/Details/5
        [Authorize(Roles = "Admin,Client")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var label = await _context.Label
                .Include(x => x.Artists)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (label == null)
            {
                return NotFound();
            }

            return View(label);
        }

        // GET: Labels/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["artists"] = new MultiSelectList(_context.Artist, nameof(Artist.Id), nameof(Artist.StageName));
            return View();
        }

        // POST: Labels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Status,Country,Founded")] Label label, int[] Artists)
        {
            var date = label.Founded.Date;

            if (date.Year < 1900)
                ModelState.AddModelError("Founded", "Labels can't be founded before 1900");

            if((DateTime.Now - date).Ticks < 0)
                ModelState.AddModelError("Founded", "This date has not yet come");

            if (Artists.Length > 0)
                label.Artists = _context.Artist.Where(x => Artists.Contains(x.Id)).ToList();

            if(_context.Label.Any(x => x.Name.ToLower() == label.Name.ToLower()))
                ModelState.AddModelError("Name", $"A label named {label.Name} already exists");

            if (ModelState.IsValid)
            {
                _context.Add(label);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["artists"] = new MultiSelectList(_context.Artist, nameof(Artist.Id), nameof(Artist.StageName));
            return View(label);
        }

        // GET: Labels/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var label = await _context.Label.FindAsync(id);
            var labelFromDb = _context.Label.Include(x => x.Artists).FirstOrDefault(x => x.Id == id);
            label.Artists = labelFromDb.Artists;
            ViewData["selectedArtists"] = label.Artists.Select(x => x.Id).ToList();

            var formatedDate = label.Founded.Year.ToString();

            if (label.Founded.Month < 10)
                formatedDate +=  "-0" + label.Founded.Month;
            else
                formatedDate += "-" + label.Founded.Month;

            if (label.Founded.Day < 10)
                formatedDate += "-0" + label.Founded.Day;
            else
                formatedDate += "-" + label.Founded.Day;

            ViewData["selectedDate"] = formatedDate;

            if (label == null)
            {
                return NotFound();
            }
            return View(label);
        }

        // POST: Labels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Status,Country,Founded")] Label label, int[] Artists)
        {
            if (id != label.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var date = label.Founded;
                var country = label.Country;
                var status = label.Status;
                label = _context.Label.Include(x => x.Artists).FirstOrDefault(x => x.Id == label.Id);
                label.Founded = date;
                label.Country = country;
                label.Status = status;
                label.Artists = _context.Artist.Where(x => Artists.Contains(x.Id)).ToList();

                try
                {
                    _context.Update(label);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LabelExists(label.Id))
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
            return View(label);
        }

        // GET: Labels/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var label = await _context.Label
                .FirstOrDefaultAsync(m => m.Id == id);
            if (label == null)
            {
                return NotFound();
            }

            return View(label);
        }

        // POST: Labels/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var label = await _context.Label.FindAsync(id);
            _context.Label.Remove(label);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LabelExists(int id)
        {
            return _context.Label.Any(e => e.Id == id);
        }
    }
}
