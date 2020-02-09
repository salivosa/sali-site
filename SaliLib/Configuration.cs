using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SaliLib
{
    public static class Configuration
    {
        public static Dictionary<string, object> get_Twitter_data(string id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            var url = "https://twitter.com/intent/user?user_id=" + id;
            var doc = new HtmlWeb().Load(url).DocumentNode;

            dict.Add("username", doc.SelectNodes("//span[@class='nickname']").Select(x => x.InnerText.Trim()).FirstOrDefault());
            dict.Add("user_title", doc.SelectNodes("//a[@rel='me']").Select(x => x.InnerText.Replace("\n", "").Trim()).FirstOrDefault());
            dict.Add("user_photo", doc.SelectNodes("//img[@class='photo']").Select(x => x.GetAttributeValue("src", "")).FirstOrDefault());

            return dict;
        }
    }
}
