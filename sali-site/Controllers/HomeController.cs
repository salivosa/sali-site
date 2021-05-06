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
        private static Dictionary<string,string> dict { get; set; }

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            if (dict == null)
                dict = SaliLib.Configuration.GetTwitterData(_configuration["twitter_id"]);

            ViewData["user_title"] = dict["twitter_nickname"];
            ViewData["user_photo"] = dict["twitter_image"];
            ViewData["twitter_page"] = dict["twitter_url"];

            ViewData["youtube_url"] = _configuration["Youtube"];
            ViewData["discord_url"] = _configuration["Discord"];
            ViewData["github_url"] = _configuration["Github"];
            ViewData["email"] = _configuration["Mail"];

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
