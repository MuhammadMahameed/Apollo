using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apollo.Services;
using Microsoft.AspNetCore.Mvc;

namespace Apollo.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly SongService _songService;

        public StatisticsController(SongService songService)
        {;
            _songService = songService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
