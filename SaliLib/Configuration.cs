using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SaliLib
{
    public class Configuration
    {
        //get Twitter Data
        public static Dictionary<string, object> get_Twitter_data(string id)
        {
            var url = "https://twitter.com/intent/user?user_id=" + id;
            var doc = new HtmlWeb().Load(url).DocumentNode;

            var dict = new Dictionary<string, object>()
            {
                { "username", doc.SelectNodes("//span[@class='nickname']").Select(x => x.InnerText.Trim()).FirstOrDefault() },
                { "user_title", doc.SelectNodes("//a[@rel='me']").Select(x => x.InnerText.Replace("\n", "").Trim()).FirstOrDefault() },
                { "user_photo", doc.SelectNodes("//img[@class='photo']").Select(x => x.GetAttributeValue("src", "")).FirstOrDefault() }
            };

            dict.Add("twitter_page", "https://twitter.com/" + dict["username"].ToString().Replace("@", ""));

            return dict;
        }
    }
}
