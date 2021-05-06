using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Configuration;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;

namespace SaliLib
{
    public class Configuration
    {
        public static Dictionary<string,string> GetTwitterData(string id)
        {
            var json = new WebClient().DownloadString("https://api.microlink.io/?url=https://twitter.com/intent/user?user_id=" + id);
            var data = JObject.Parse(json);

            var twitter_data = data["data"];

            var twitter_title = (string)twitter_data["title"];

            var twitter_nickname = twitter_title.Split("(")[0].Trim();
            var twitter_username = twitter_title.Split("(")[1].Split(")")[0].Replace("@", "");

            var twitter_url = (string)twitter_data["url"];

            var twitter_image_url = (string)twitter_data["image"]["url"];
            twitter_image_url = twitter_image_url.Replace("_normal", "_400x400");

            var dict = new Dictionary<string, string>();
            dict.Add("twitter_url", twitter_url);
            dict.Add("twitter_nickname", twitter_nickname);
            dict.Add("twitter_username", twitter_username);
            dict.Add("twitter_image", twitter_image_url);

            return dict;
        }
    }
}
