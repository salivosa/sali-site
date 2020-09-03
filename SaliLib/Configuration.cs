using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace SaliLib
{
    public class Configuration
    {
        //get Twitter Data
        public static Dictionary<string, object> get_Twitter_data(string id)
        {
            // find chromedrive exe in binaries
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            //open driver headless
            var options = new ChromeOptions();
            options.AddArgument("headless");
            var driver = new ChromeDriver(path, options);

            //navigate to page
            driver.Navigate().GoToUrl("https://twitter.com/intent/user?user_id=" + id);

            Thread.Sleep(10 * 1000);

            //get html data from loaded page
            var html_data = driver.PageSource;

            //quit page
            driver.Quit();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html_data);

            var url = "https://twitter.com/intent/user?user_id=" + id;
            var node = doc.DocumentNode;

            var dict = new Dictionary<string, object>()
            {
                { "user_title", node.SelectNodes("//title").Select(x => x.InnerText.Split(" (")[0].Trim()).FirstOrDefault() },
                { "twitter_page", "https://twitter.com/" +  node.SelectNodes("//title").Select(x => x.InnerText.Split("(")[1].Replace("@", "").Trim()).FirstOrDefault() },
                { "user_photo", node.SelectNodes("//meta[@property='og:image']").Select(x => x.GetAttributeValue("content", "").Replace("normal", "200x200")).FirstOrDefault() },
                { "user_id",  id}
            };

            return dict;
        }
    }
}
