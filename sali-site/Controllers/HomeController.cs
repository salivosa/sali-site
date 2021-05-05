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
                
            }  
        }

        public IActionResult Index()
        {
 
            ViewData["user_title"] = _configuration["twitter_user_title"];
            ViewData["user_photo"] = _configuration["twitter_user_photo"];
            ViewData["twitter_page"] = _configuration["twitter_page"];

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
