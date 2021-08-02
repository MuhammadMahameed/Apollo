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

        public async Task<JsonResult> GetTimelineEmbed(string url)
        {
            return Json(await _twitterService.GetTimelineEmbed(url));
        }

        public void PostTweet(string tweet)
        {
            _twitterService.PostTweet(tweet);
        }
    }
}
