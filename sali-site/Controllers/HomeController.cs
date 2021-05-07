using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using sali_site.Models;
using SaliLib;

namespace sali_site.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private static Dictionary<string,string> dict { get; set; }

        private System.Threading.Timer timer;
        private void SetUpTimer(TimeSpan alertTime)
        {
            DateTime current = DateTime.Now;
            TimeSpan timeToGo = alertTime - current.TimeOfDay;

            if (timeToGo < TimeSpan.Zero)
                return;

            timer = new System.Threading.Timer(x =>
            {
                dict = SaliLib.Configuration.GetTwitterData(_configuration["twitter_id"]);

            }, null, timeToGo, Timeout.InfiniteTimeSpan);
        }

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _configuration = configuration;

            if (dict == null)
                dict = SaliLib.Configuration.GetTwitterData(_configuration["twitter_id"]);

            if (timer == null)
                SetUpTimer(new TimeSpan(23, 59, 59));

        }

        public IActionResult Index()
        {
            ViewData["user_title"] = dict["twitter_nickname"];
            ViewData["user_photo"] = dict["twitter_image"];
            ViewData["twitter_page"] = dict["twitter_url"];

            ViewData["youtube_url"] = _configuration["Youtube"];
            ViewData["discord_url"] = _configuration["Discord"];
            ViewData["github_url"] = _configuration["Github"];
            ViewData["email"] = _configuration["Mail"];

            return View();
        }

        public RedirectResult Refresh()
        {
            dict = SaliLib.Configuration.GetTwitterData(_configuration["twitter_id"]);
            return Redirect("/Home/Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
