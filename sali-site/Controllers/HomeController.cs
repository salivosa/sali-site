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

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            if (config == null)
            {
                config = new SaliLib.Configuration(SaliLib.Configuration.unencrypt_key(_configuration["consumerKey"]), SaliLib.Configuration.unencrypt_key(_configuration["consumerSecret"]), SaliLib.Configuration.unencrypt_key(_configuration["tokenValue"]), SaliLib.Configuration.unencrypt_key(_configuration["tokenSecret"]));

                //Useful timer i might use later

                //Timer for update variable every 60 minutes
                //var aTimer = new Timer(60 * 60 * 1000);
                //aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                //aTimer.Start();
            }
        }

        private static Dictionary<string, object> twitter_data { get; set; }

        //Task which will be handly in a future
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
        }

        public IActionResult Index()
        {
            var TwitterUserId = SaliLib.Configuration.unencrypt_key(_configuration["Twitter ID"]);

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

        [HttpGet]
        public void RefreshTwitterData()
        {

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
