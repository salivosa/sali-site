using System;
using System.Collections.Generic;
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

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            if (twitter_data == null)
            {
                //Timer for update variable every 60 minutes
                var aTimer = new Timer(60 * 60 * 1000);
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                aTimer.Start();
            }
        }

        private static Dictionary<string, object> twitter_data { get; set; }

        //Every hour, update variable twitter_data
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            twitter_data = Configuration.get_Twitter_data(twitter_data["user_id"].ToString());
        }

        public IActionResult Index()
        {
            var TwitterUserId = _configuration.GetSection("Twitter ID").Get<string>();

            if (twitter_data == null)
                twitter_data = Configuration.get_Twitter_data(TwitterUserId);

            ViewData["user_title"] = twitter_data["user_title"];
            ViewData["user_photo"] = twitter_data["user_photo"];
            ViewData["twitter_page"] = twitter_data["twitter_page"];

            ViewData["youtube_url"] = _configuration.GetSection("Youtube").Get<string>();
            ViewData["discord_url"] = _configuration.GetSection("Discord").Get<string>();
            ViewData["github_url"] = _configuration.GetSection("Github").Get<string>();
            ViewData["email"] = _configuration.GetSection("Mail").Get<string>();

            return View();
        }

        [HttpGet]
        public void RefreshTwitterData()
        {
            twitter_data = Configuration.get_Twitter_data(twitter_data["user_id"].ToString());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
