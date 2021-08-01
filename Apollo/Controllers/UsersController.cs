using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Apollo.Data;
using Apollo.Models;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Apollo.Services;
using Microsoft.AspNetCore.Authorization;

namespace Apollo.Controllers
{
    public class UsersController : Controller
    {
        private readonly DataContext _context;
        private readonly UserService _userService;

        public UsersController(DataContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Index), "Home");
        }

        // GET: Users/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Users/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,Username,Password,Age,EmailAdress")] User user)
        {
            var userTaken = _context.User.FirstOrDefault(u => u.Username == user.Username);
            var emailTaken = _context.User.FirstOrDefault(u => u.EmailAdress == user.EmailAdress);
            if (emailTaken != null)
            {
                ModelState.AddModelError("EmailAdress", "EmailAdress already taken");
            }
            if (userTaken != null)
            {
                ModelState.AddModelError("Username", "Username already taken");
            }
            if (!(Regex.IsMatch(user.Password, @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,16}$")))
            {
                ModelState.AddModelError("Password", "Password has to contain Numeric, Capital and  between 8-16 chars");
            }
              
               
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                var q = _context.User.FirstOrDefault(x => x.Username == user.Username && x.Password == user.Password);
                Signin(q);
                return RedirectToAction(nameof(Index), "Home");
            }
            return View(user);
        }
        // GET: Users/Login
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

        // POST: Users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Id,Username,Password")] User user)
        {
            var q = _context.User.FirstOrDefault(x => x.Username == user.Username && x.Password == user.Password);
            if (q == null)
            {
                ModelState.AddModelError("Password", "Username and/or password incorrect");
            }
            ModelState.Remove("EmailAdress");
            ModelState.Remove("Age");
            if (ModelState.IsValid)
            {
                Signin(q);
                return RedirectToAction(nameof(Index), "Home");
            }
            return View(user);
        }

        private async void Signin(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.Role,user.RoleType.ToString()),
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties{
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);
        }

        public IActionResult GetAllUsers()
        {
            return Json(_userService.GetAllUsers());
        }

        public IActionResult Filter(string matchingStr)
        {
            return Json(_userService.FilterUsers(matchingStr));
        }

        // GET: Users1
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: Users1/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users1/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Username,Password,Age,EmailAdress,RoleType")] User user)
        {
            var userTaken = _context.User.FirstOrDefault(u => u.Username == user.Username);
            var emailTaken = _context.User.FirstOrDefault(u => u.EmailAdress == user.EmailAdress);
            if (emailTaken != null)
            {
                ModelState.AddModelError("EmailAdress", "EmailAdress already taken");
            }
            if (userTaken != null)
            {
                ModelState.AddModelError("Username", "Username already taken");
            }
            if (!(Regex.IsMatch(user.Password, @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,16}$")))
            {
                ModelState.AddModelError("Password", "Password has to contain Numeric, Capital and  between 8-16 chars");
            }

            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users1/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Password,Age,EmailAdress,RoleType")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: Users1/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
