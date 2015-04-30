using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentimentAnalysis
{
    /// <summary>
    /// Represents a Twitter Handle for which we store data.
    /// </summary>
    class TwitterHandle
    {
        public string Name { get; set; }
        public int Followers { get; set; }
        public int Retweets { get; set; }
        public int Score { get; set; }

        public TwitterHandle (string Name)
        {
            this.Name = Name;
        }
    }
}
