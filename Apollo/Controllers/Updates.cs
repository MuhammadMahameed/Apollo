using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Apollo.Models;
using Apollo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Apollo.Controllers
{
    public class Updates : Controller
    {
        private readonly SpotifyService _spotifyService;
        private readonly SpotifyReleasesService _spotifyReleasesService;
        private readonly IConfiguration _configuration;

        public Updates(SpotifyService spotifyService, IConfiguration configuration, SpotifyReleasesService spotifyReleasesService)
        {
            _spotifyService = spotifyService;
            _spotifyReleasesService = spotifyReleasesService;
            _configuration = configuration;
        }

        [Authorize(Roles = "Admin,Client")]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IEnumerable<Release>> GetReleases(int limit)
        {
            try
            {
                var token = await _spotifyService.GetToken(_configuration["Spotify:client_id"], _configuration["Spotify:client_secret"]);
                var newReleases = await _spotifyReleasesService.GetNewReleases(limit, token);
                return newReleases;
            }
            catch(Exception ex)
            {
                Debug.Write(ex);
                return Enumerable.Empty<Release>();
            }
        }
    }
}
