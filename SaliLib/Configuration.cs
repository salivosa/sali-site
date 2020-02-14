using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.ViewManagement;

namespace SaliLib
{
    public class Configuration
    {
        public static bool check_dark_mode = _check_dark_mode();

        //get Twitter Data
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

        //Get if Windows OS has dark theme on
        public static bool _check_dark_mode()
        {
            var settings = new UISettings();
            var foreground = settings.GetColorValue(UIColorType.Foreground).ToString();
            var background = settings.GetColorValue(UIColorType.Background).ToString();

            bool check = foreground == "#FFFFFFFF" && background == "#FF000000";

            return check;
        }
    }
}
