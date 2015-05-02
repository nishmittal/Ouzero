using SentimentAnalysis.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Interfaces;

namespace SentimentAnalysis
{
    /// <summary>
    /// Twitter-oriented implementation of the <see cref="IDataSourcer"/>interface.
    /// </summary>
    class TwitterDataSourcer : IDataSourcer
    {
        private string AccessToken = "3226815046-jOHYCioNDa0m3oLoukS35xY6DLsWQHbQRETZoPq";
        private string AccessTokenSecret = "XKUWVQgKEe2wD0wmLeCF5senwKqPAfcQQZEX5XYtNLQRs";
        private string ConsumerKey = "nysITef21Ph7H5gb3mQaCBYXL";
        private string ConsumerSecret = "x1ZAAXTxFeZcNN4yxQOC3sIESRTTtsJpwxKgsiBIcUnaGqH6Ap";

        public IList<TwitterHandle> Handles { get; set; }

        public TwitterDataSourcer(IList<TwitterHandle> handles)
        {
            this.Handles = handles;
            TwitterCredentials.SetCredentials( AccessToken, AccessTokenSecret, ConsumerKey, ConsumerSecret );
        }

        public void GetData()
        {
            foreach ( TwitterHandle h in Handles )
            {
                var user = User.GetUserFromScreenName( h.Name );
                h.Followers = user.FollowersCount;
                h.Friends = user.FriendsCount;
                // for retweets per tweet, get list of tweets, then cycle through each one and sum up the number of retweets and then simply divide

                h.RetweetRate = 0;
                // for favourites per tweet, do the same as above.
                h.FavouriteRate = 0;
                h.Score = ComputeScore( h );
            }
        }

        private static int ComputeScore(TwitterHandle h)
        {
            // score = (retweets x favourites)^3 x (friends/followers)
            var score = Math.Pow( h.RetweetRate * h.FavouriteRate, 3 ) * ( h.Friends / h.Followers );
            return (int) score;
        }

        // TEST THIS! =)
        private static ITweet[] GetUserTimelineTweets( string userName, int maxNumberOfTweets )
        {
            var tweets = new List<ITweet>();

            var receivedTweets = Timeline.GetUserTimeline( userName, 200 ).ToArray();
            tweets.AddRange( receivedTweets );

            while ( tweets.Count < maxNumberOfTweets && receivedTweets.Length == 200 )
            {
                var oldestTweet = tweets.Min( x => x.Id );
                var userTimelineParameter = Timeline.CreateUserTimelineRequestParameter( userName );
                userTimelineParameter.MaxId = oldestTweet;
                userTimelineParameter.MaximumNumberOfTweetsToRetrieve = 200;

                receivedTweets = Timeline.GetUserTimeline( userTimelineParameter ).ToArray();
                tweets.AddRange( receivedTweets );
            }

            return tweets.Distinct().ToArray();
        }

    }
}
