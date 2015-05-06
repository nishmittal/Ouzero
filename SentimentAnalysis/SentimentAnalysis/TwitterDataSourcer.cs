using System;
using System.Collections.Generic;
using System.Linq;
using SentimentAnalysis.Interfaces;
using Tweetinvi;
using Tweetinvi.Core.Interfaces;

namespace SentimentAnalysis
{
    /// <summary>
    /// Twitter-oriented implementation of the <see cref="IDataSourcer"/>interface.
    /// </summary>
    public class TwitterDataSourcer : IDataSourcer
    {
        private static string _accessToken = "3226815046-jOHYCioNDa0m3oLoukS35xY6DLsWQHbQRETZoPq";
        private static string _accessTokenSecret = "XKUWVQgKEe2wD0wmLeCF5senwKqPAfcQQZEX5XYtNLQRs";
        private static string _consumerKey = "nysITef21Ph7H5gb3mQaCBYXL";
        private static string _consumerSecret = "x1ZAAXTxFeZcNN4yxQOC3sIESRTTtsJpwxKgsiBIcUnaGqH6Ap";

        public IList<TwitterHandle> Handles { get; set; }

        public TwitterDataSourcer( IList<TwitterHandle> handles )
        {
            Handles = handles;
            TwitterCredentials.SetCredentials( _accessToken, _accessTokenSecret, _consumerKey, _consumerSecret );
        }


        public static void SetCredentials()
        {
            TwitterCredentials.SetCredentials( _accessToken, _accessTokenSecret, _consumerKey, _consumerSecret );
        }

        public static IUser GetUser( string name )
        {
            return User.GetUserFromScreenName( name );
        }

        public static ITweetList[] GetUserSubscribedLists( IUser user )
        {
            var tweetLists = TweetList.GetUserLists( user, false );
            return tweetLists.ToArray();
        }

        public static IUser[] GetUsersInList( ITweetList list )
        {
            return list.GetMembers( 10000 ).ToArray();
        }

        public TwitterHandle GetPopulatedHandleFromUser( IUser user )
        {
            var h = new TwitterHandle( user.Name )
            {
                Followers = user.FollowersCount,
                Friends = user.FriendsCount
            };

            var numberOfTweets = 500;
            var tweets = GetUserTimelineTweets( h.Name, numberOfTweets );

            if ( tweets.Length != 500 ) // in case the account doesn't have the required amount
            {
                numberOfTweets = tweets.Length;
            }

            var totalRetweets = 0;
            var totalFavourites = 0;

            foreach ( var t in tweets )
            {
                totalRetweets += t.RetweetCount;
                totalFavourites += t.FavouriteCount;
            }

            h.RetweetRate = totalRetweets / numberOfTweets;
            h.FavouriteRate = totalFavourites / numberOfTweets;

            return h;
        }

        /// <summary>
        /// Needs more work.
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        public static double ComputeScoreStatic( TwitterHandle h )
        {
            var score = Math.Pow( h.RetweetRate, 2 ) + h.FavouriteRate + h.Friends / 100 + ( h.Followers / 1000 );
            return score;
        }

        public static ITweet[] GetUserTimelineTweets( string userName, int maxNumberOfTweets )
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
