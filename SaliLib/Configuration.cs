using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Configuration;
using TwitterOps;
using CodificacionBinariaLib;
using System.Threading.Tasks;
using System.Net.Http;

namespace SaliLib
{
    public class Configuration
    {
        public static Operations ops { get; set; }
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
                { "user_title", user.name },
                { "twitter_page", "https://twitter.com/" +  user.username },
                { "user_photo", user.profile_image_url }
            };

            return dict;
        }

        public static string unencrypt_key(string encrypted_key)
        {
            return Manejador_Binario.obtenerCadenaLiteral(encrypted_key);
        }

        public async Task keep_session_alive()
        {
            string url = "http://salivosa.xyz";

            var client = new HttpClient();

            await client.GetAsync(url);
        }

        public void twitter_bot_handler()
        {
            try
            {
                var response = ops.Tweets.IsLastMentionRepliedByLoggedUser();

                if (!response.Item1)
                    ops.Tweets.PostReplyTweet(response.Item2.tweet_message, response.Item2);
                
            }
            catch (Exception)
            {

            }
        }
    }
}
