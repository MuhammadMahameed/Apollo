using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apollo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Apollo.Controllers
{
    public class TwitterController : Controller
    {
        private readonly TwitterService _twitterService;

        public TwitterController(TwitterService twitterService)
        {
            _twitterService = twitterService;
        }

        /*
        public async Task<IEnumerable> GetTweets()
        {
            return await _twitterService.GetTweets();
        }
        */

        public async Task<JsonResult> GetTimelineEmbed(string url)
        {
            return Json(await _twitterService.GetTimelineEmbed(url));
        }

        // GET: TwitterController
        public ActionResult Index()
        {
            return View();
        }

        // GET: TwitterController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TwitterController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TwitterController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TwitterController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TwitterController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TwitterController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TwitterController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
