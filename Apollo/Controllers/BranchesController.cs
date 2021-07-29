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
    public class BranchesController : Controller
    {
        private readonly DataContext _context;
        private readonly BranchService _branchService;

        public BranchesController(DataContext context, BranchService branchService)
        {
            _context = context;
            _branchService = branchService;
        }

        public async Task<IActionResult> GetBranches()
        {
            return Json(await _context.Branch.ToListAsync());
        }

        public async Task<byte[]> GetBingMap(string coordinates)
        {
            return await _branchService.GetBingMap(coordinates);
        }

        public IActionResult Filter(string matchingStr)
        {
            return Json(_branchService.FilterBranches(matchingStr));
        }

        // GET: Branches
        public async Task<IActionResult> Index()
        {
            return View(await _context.Branch.ToListAsync());
        }

        // GET: Branches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _context.Branch
                .FirstOrDefaultAsync(m => m.Id == id);
            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }

        // GET: Branches/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Branches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AddressName,Coordinate")] Branch branch)
        {
            var addressName = _context.Branch.FirstOrDefault(x => x.AddressName == branch.AddressName);
            var coordinate = _context.Branch.FirstOrDefault(x => x.Coordinate == branch.Coordinate);

            if (addressName != null)
            {
                ModelState.AddModelError("AddressName", "This branch already exists");
            }

            if (coordinate != null)
            {
                ModelState.AddModelError("Coordinate", "This coordinate is already associated to another branch");
            }

            if (ModelState.IsValid)
            {
                _context.Add(branch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(branch);
        }

        // GET: Branches/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _context.Branch.FindAsync(id);
            if (branch == null)
            {
                return NotFound();
            }
            return View(branch);
        }

        // POST: Branches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AddressName,Coordinate")] Branch branch)
        {
            if (id != branch.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(branch);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BranchExists(branch.Id))
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
            return View(branch);
        }

        // GET: Branches/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _context.Branch
                .FirstOrDefaultAsync(m => m.Id == id);
            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }

        // POST: Branches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var branch = await _context.Branch.FindAsync(id);
            _context.Branch.Remove(branch);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BranchExists(int id)
        {
            return _context.Branch.Any(e => e.Id == id);
        }
    }
}
