using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private static SaliLib.Configuration config { get; set; }
        private static Timer timer_twitter { get; set; }
        private static Timer timer_session_alive { get; set; }

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            if (config == null)
            {
                config = new SaliLib.Configuration(SaliLib.Configuration.unencrypt_key(_configuration["consumerKey"]), SaliLib.Configuration.unencrypt_key(_configuration["consumerSecret"]), SaliLib.Configuration.unencrypt_key(_configuration["tokenValue"]), SaliLib.Configuration.unencrypt_key(_configuration["tokenSecret"]));

                //Twitter Timer
                timer_twitter = new Timer();

                timer_twitter.Interval = 30000;
                timer_twitter.Elapsed += checkTwitter;

                timer_twitter.Enabled = true;

                //Keep Session Alive Timer
                timer_session_alive = new Timer();

                timer_session_alive.Interval = 600000;
                timer_session_alive.Elapsed += keepSessionAlive;

                timer_session_alive.Enabled = true;
            }  
        }

        public IActionResult Index()
        {
            var TwitterUserId = _configuration["Twitter ID"];

            var twitter_data = config.get_Twitter_data(TwitterUserId);

            ViewData["user_title"] = twitter_data["user_title"];
            ViewData["user_photo"] = twitter_data["user_photo"];
            ViewData["twitter_page"] = twitter_data["twitter_page"];

            ViewData["youtube_url"] = _configuration["Youtube"];
            ViewData["discord_url"] = _configuration["Discord"];
            ViewData["github_url"] = _configuration["Github"];
            ViewData["email"] = _configuration["Mail"];

            return View();
        }

        private void checkTwitter(object sender, ElapsedEventArgs e)
        {
            timer_twitter.Enabled = false;

            config.twitter_bot_handler();
            GC.Collect();

            timer_twitter.Enabled = true;

        }

        private async void keepSessionAlive(object sender, ElapsedEventArgs e)
        {
            timer_session_alive.Enabled = false;

            await config.keep_session_alive();
            GC.Collect();

            timer_session_alive.Enabled = true;

        }

        /*
        [HttpGet]
        public void RefreshTwitterData()
        {

        }
        */

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
