using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;

namespace SentimentAnalysis
{
    class Program
    {
        private IList<TwitterHandle> Handles;
        private string AccessToken = "3226815046-jOHYCioNDa0m3oLoukS35xY6DLsWQHbQRETZoPq";
        private string AccessTokenSecret = "XKUWVQgKEe2wD0wmLeCF5senwKqPAfcQQZEX5XYtNLQRs";
        private string ConsumerKey = "nysITef21Ph7H5gb3mQaCBYXL";
        private string ConsumerSecret = "x1ZAAXTxFeZcNN4yxQOC3sIESRTTtsJpwxKgsiBIcUnaGqH6Ap";

        static void Main(string[] args)
        {
            var prg = new Program();
            prg.Go();
        }

        public Program()
        {
            Handles = new List<TwitterHandle>();
            Handles.Add(new TwitterHandle("@NetshockTech"));
            Handles.Add(new TwitterHandle("@techcrunch"));
            TwitterCredentials.SetCredentials(AccessToken, AccessTokenSecret, ConsumerKey, ConsumerSecret);
        }

        private void Go()
        {
            
            foreach (TwitterHandle h in Handles)
            {
                var user = User.GetUserFromScreenName(h.Name);
                h.Followers = user.FollowersCount;
                h.Friends = user.FriendsCount;
                h.Retweets = 0;
                var score = ComputeScore(h);
                h.Score = score;
            }
        }

        /// <summary>
        /// Stubbed.
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        private int ComputeScore(TwitterHandle h)
        {
            return 80;
        }

        /// <summary>
        /// Stubbed.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private int GetFavourites(string name)
        {
            return 50;
        }

        /// <summary>
        /// Stubbed.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private int GetRetweets(string p)
        {
            return 100;
        }

        /// <summary>
        /// Stubbed.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private int GetFollowers(string p)
        {
            return 10;
        }
    }
}
