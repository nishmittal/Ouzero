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

        /// <summary>
        /// Stores a copy of the handles supplied and sets the credentials for use.
        /// </summary>
        /// <param name="handles"></param>
        public TwitterDataSourcer( IList<TwitterHandle> handles )
        {
            Handles = handles;
            TwitterCredentials.SetCredentials( _accessToken, _accessTokenSecret, _consumerKey, _consumerSecret );
        }


        public static void SetCredentials()
        {
            TwitterCredentials.SetCredentials( _accessToken, _accessTokenSecret, _consumerKey, _consumerSecret );
        }

        public static List<TwitterHandle> GetScoredHandlesFromUserLists( string name )
        {
            var scoredHandles = new List<TwitterHandle>();

            var ouzero = new TwitterHandle( name );
            var ouzeroUser = GetUser( ouzero.Name );
            var userLists = GetUserSubscribedLists( ouzeroUser );

            foreach ( var tweetList in userLists )
            {
                var usersInList = GetUsersInList( tweetList );
                foreach ( var user in usersInList )
                {
                    var populatedHandle = GetPopulatedHandleFromUser( user );
                    if ( populatedHandle.Name != "invisible" )
                    {
                        scoredHandles.Add( populatedHandle );
                    }
                }
            }

            return scoredHandles;
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

        public static TwitterHandle GetPopulatedHandleFromUser( IUser user )
        {
            var h = new TwitterHandle( user.ScreenName)
            {
                Followers = user.FollowersCount,
                Friends = user.FriendsCount
            };

            var numberOfTweets = 200;

            ITweet[] tweets;
            try
            {
                tweets = GetUserTimelineTweets( h.Name, numberOfTweets );
            }
            catch ( Exception ) //something went wrong
            {
                return GetInvisibleUser();
            }

            if ( tweets.Length != 500 ) // in case the account doesn't have the required amount
            {
                if ( tweets.Length == 0 )
                {
                    return GetInvisibleUser();
                }
                numberOfTweets = tweets.Length;
            }

            var totalRetweets = 0;
            var totalFavourites = 0;

            foreach ( var t in tweets )
            {
                totalRetweets += t.RetweetCount;
                totalFavourites += t.FavouriteCount;
            }

            h.RetweetRate = (totalRetweets / numberOfTweets) + 1;
            h.FavouriteRate = (totalFavourites / numberOfTweets) + 1;

            h.Score = ComputeScoreStatic( h );

            return h;
        }

        private static TwitterHandle GetInvisibleUser()
        {
            return new TwitterHandle("invisible");
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

            var user = User.GetUserFromScreenName( userName );
            var tweets1 = Timeline.GetUserTimeline( user, 200 );

            //var requestParameter = Timeline.CreateUserTimelineRequestParameter( user );
            //requestParameter.MaxId = tweets1.Min( x => x.Id ) - 1;
            //requestParameter.MaximumNumberOfTweetsToRetrieve = 200;
            //var tweets2 = Timeline.GetUserTimeline( requestParameter );

            //var tweets = tweets1.Concat( tweets2 );

            return tweets1.ToArray();
        }

    }
}
