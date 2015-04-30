using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentimentAnalysis
{
    class Program
    {
        private IList<TwitterHandle> Handles;

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
        }

        private void Go()
        {
            foreach(TwitterHandle h in Handles)
            {
                var name = h.Name;
                h.Followers = GetFollowers(name);
                h.Retweets = GetRetweets(name);
                h.Favourites = GetFavourites(name);
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
