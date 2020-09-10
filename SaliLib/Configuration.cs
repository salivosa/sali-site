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

        public async Task twitter_bot_handler()
        {
            try
            {
                var response = await ops.Tweets.IsLastMentionRepliedByLoggedUserAsync();

                if (!response.Item1)
                {
                    await ops.Tweets.PostReplyTweetAsync(response.Item2.tweet_message, response.Item2);
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
