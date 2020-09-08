using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Configuration;
using TwitterOps;

namespace SaliLib
{
    public class Configuration
    {
        private static Operations ops { get; set; }
        public Configuration(string consumerKey, string consumerSecret, string tokenValue, string tokenSecret)
        {
            ops = new Operations(consumerKey, consumerSecret, tokenValue, tokenSecret);
        }

        //get Twitter Data
        public Dictionary<string, object> get_Twitter_data(string id)
        {

            var user = ops.Users.GetUserById(id);

            var dict = new Dictionary<string, object>()
            {
                { "user_title", user.username },
                { "twitter_page", "https://twitter.com/" +  user.username },
                { "user_photo", user.profile_image_url }
            };

            return dict;
        }
    }
}
