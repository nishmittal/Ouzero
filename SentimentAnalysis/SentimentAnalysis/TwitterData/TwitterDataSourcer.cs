using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tweetinvi;
using Tweetinvi.Core.Interfaces;

namespace SentimentAnalysis.TwitterData
{
    /// <summary>
    /// Twitter-oriented implementation.
    /// </summary>
    public static class TwitterDataSourcer
    {
        private static string _accessToken;
        private static string _accessTokenSecret;
        private static string _consumerKey;
        private static string _consumerSecret;

        public static readonly IList<TwitterHandle> MissingHandles = new List<TwitterHandle>();
        private static Dictionary<string, string> _keys;

        public static void SetCredentials()
        {
            PopulateKeysDictionary();
            _keys.TryGetValue("AccessToken", out _accessToken);
            _keys.TryGetValue("AccessTokenSecret", out _accessTokenSecret);
            _keys.TryGetValue("ConsumerKey", out _consumerKey);
            _keys.TryGetValue("ConsumerSecret", out _consumerSecret);
            TwitterCredentials.SetCredentials( _accessToken, _accessTokenSecret, _consumerKey, _consumerSecret );
        }

        public static List<TwitterHandle> GetScoredHandlesFromUserLists( string name )
        {
            var scoredHandles = new List<TwitterHandle>();

            var ouzero = new TwitterHandle( name );
            var ouzeroUser = GetUser( ouzero.Username );
            var userLists = GetUserSubscribedLists( ouzeroUser );

            foreach ( var tweetList in userLists )
            {
                scoredHandles.AddRange( GetScoredHandlesFromUserList( tweetList.Slug, tweetList.Creator.ScreenName ) );
            }

            return scoredHandles;
        }

        public static List<TwitterHandle> GetScoredHandlesFromUserList( string listName, string listCreator )
        {
            var usersInList = GetUsersFromList(listName, listCreator);

            return GetScoredHandlesFromUsers(usersInList);
        }

        public static IEnumerable<IUser> GetUsersFromList(string listName, string listCreator)
        {
            var tweetList = TweetList.GetExistingList(listName, listCreator);
            var usersInList = GetUsersInList(tweetList);
            return usersInList;
        }


        private static List<TwitterHandle> GetScoredHandlesFromUsers(IEnumerable<IUser> users)
        {
            var scoredHandles = new List<TwitterHandle>();
            foreach ( var user in users )
            {
                if (user == null)
                    continue;

                var populatedHandle = GetPopulatedHandleFromUser( user );
                if ( populatedHandle.Username.StartsWith( "invisible" ) )
                {
                    MissingHandles.Add( new TwitterHandle( user.ScreenName ) );
                }
                else
                {
                    scoredHandles.Add( populatedHandle );
                }
            }

            return scoredHandles;
        }

        public static List<TwitterHandle> GetScoredHandlesFromUsernames(List<string> usernames )
        {
            // TODO VERIFY THE FIRST ONE WORKS
            var userList = usernames.Select(GetUser).ToList();
            var usersList = new List<IUser>();

            foreach (var username in usernames)
            {
                usersList.Add(GetUser(username));
            }

            return GetScoredHandlesFromUsers(userList);
        }

        private static IUser GetUser( string name )
        {
            return User.GetUserFromScreenName( name );
        }

        private static ITweetList[] GetUserSubscribedLists( IUser user )
        {
            var tweetLists = TweetList.GetUserLists( user, false );
            return tweetLists.ToArray();
        }

        private static IEnumerable<IUser> GetUsersInList( ITweetList list )
        {
            return list.GetMembers( 10000 );
        }

        private static string GetLocation(IUser user)
        {
            var loc = user.Location;
            return loc.Equals(string.Empty) ? "unspecified" : loc;
        }

        private static string GetBio(IUser user)
        {
            var bio = user.Description;
            return bio.Equals(string.Empty) ? "unspecified" : bio;
        }

        private static string GetWebsite(IUser user)
        {
            var website = user.Url;
            if (website == null || website.Equals(string.Empty))
                return "unspecified";
            return website;
        }

        private static TwitterHandle GetPopulatedHandleFromUser( IUser user )
        {
            var h = new TwitterHandle( user.ScreenName )
            {
                Name = user.Name,
                ImgUrl = user.ProfileImageUrl,
                Followers = user.FollowersCount,
                Friends = user.FriendsCount,
                Location = GetLocation(user),
                Website = GetWebsite(user)
            };

            var desc = GetBio(user);
            desc = desc.Replace("\r\n", " ");
            desc = desc.Replace( "\n", " " );
            desc = desc.Replace("\r", " ");
            h.Bio = desc;

            var numberOfTweets = 200;

            ITweet[] tweets;
            try
            {
                tweets = GetUserTimelineTweets( user, numberOfTweets );
            }
            catch ( Exception e) //something went wrong
            {
                Console.WriteLine(@"Error when getting populated handle: " + e.Message);
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
                if ( !t.IsRetweet )
                    totalRetweets += t.RetweetCount;
                totalFavourites += t.FavouriteCount;
            }

            h.RetweetRate = ( totalRetweets / numberOfTweets ) + 1;
            h.FavouriteRate = ( totalFavourites / numberOfTweets ) + 1;

            h.Score = ComputeScoreStatic( h );

            return h;
        }

        private static TwitterHandle GetInvisibleUser()
        {
            var t = new TwitterHandle("invisible")
            {
                Bio = "",
                Location = "",
                ImgUrl = ""
            };
            return t;
        }

        /// <summary>
        /// Needs more work.
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        private static double ComputeScoreStatic( TwitterHandle h )
        {
            var score = Math.Pow( h.RetweetRate, 2 ) + h.FavouriteRate + h.Friends / 100 + ( h.Followers / 1000 );
            return score;
        }

        /// <summary>
        /// Gets a number of tweets up to the maximum specified for a given user. Excludes retweets of other users.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="maxNumberOfTweets"></param>
        /// <returns></returns>
        private static ITweet[] GetUserTimelineTweets( IUser user, int maxNumberOfTweets )
        {
            var tweets = new List<ITweet>();

            var userTimelineParameter = Timeline.CreateUserTimelineRequestParameter( user );
            userTimelineParameter.IncludeRTS = false;
            userTimelineParameter.MaximumNumberOfTweetsToRetrieve = maxNumberOfTweets;
            var receivedTweets = Timeline.GetUserTimeline( userTimelineParameter ).ToArray();

            tweets.AddRange( receivedTweets );

            while ( tweets.Count < maxNumberOfTweets && receivedTweets.Length == 200 )
            {
                var oldestTweet = tweets.Min( x => x.Id );
                userTimelineParameter.MaxId = oldestTweet;
                userTimelineParameter.MaximumNumberOfTweetsToRetrieve = 200;

                receivedTweets = Timeline.GetUserTimeline( userTimelineParameter ).ToArray();
                tweets.AddRange( receivedTweets );
            }

            return tweets.Distinct().ToArray();
        }

        private static void PopulateKeysDictionary()
        {
            const string path = @"C:\Users\Nishant\Documents\twitterkeys.csv";
            _keys = File.ReadLines(path).Select(line => line.Split(',')).ToDictionary(line => line[0], line => line[1]);
        }
    }
}
