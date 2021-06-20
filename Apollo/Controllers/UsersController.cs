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

namespace Apollo.Controllers
{
    public class UsersController : Controller
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
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
        

    }

}
