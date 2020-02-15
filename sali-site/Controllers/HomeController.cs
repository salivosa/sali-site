using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
        }

        public IActionResult Index()
        {
            var TwitterUserId = _configuration.GetSection("Twitter ID").Get<string>();
            
            var content = Configuration.get_Twitter_data(TwitterUserId);

            ViewData["user_title"] = content["user_title"];
            ViewData["username"] = content["username"];
            ViewData["user_photo"] = content["user_photo"];
            ViewData["twitter_page"] = content["twitter_page"];

            ViewData["youtube_url"] = _configuration.GetSection("Youtube").Get<string>();
            ViewData["discord_url"] = _configuration.GetSection("Discord").Get<string>();
            ViewData["github_url"] = _configuration.GetSection("Github").Get<string>();
            ViewData["email"] = _configuration.GetSection("Mail").Get<string>();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
