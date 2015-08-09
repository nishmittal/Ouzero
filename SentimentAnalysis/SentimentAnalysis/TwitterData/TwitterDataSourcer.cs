using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using SentimentAnalysis.Csv;
using Tweetinvi;
using Tweetinvi.Core.Interfaces;
// ReSharper disable PossibleLossOfFraction

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
        private static DateTime _lastScoreTime;
        public static List<string> ScoringTimes { get; private set; }

        public static void SetCredentials()
        {
            PopulateKeysDictionary();
            _keys.TryGetValue("AccessToken", out _accessToken);
            _keys.TryGetValue("AccessTokenSecret", out _accessTokenSecret);
            _keys.TryGetValue("ConsumerKey", out _consumerKey);
            _keys.TryGetValue("ConsumerSecret", out _consumerSecret);
            TwitterCredentials.SetCredentials(_accessToken, _accessTokenSecret, _consumerKey, _consumerSecret);
        }

        public static IEnumerable<TwitterHandle> GetScoredHandlesFromUserLists(string name)
        {
            var scoredHandles = new List<TwitterHandle>();

            var ouzero = new TwitterHandle(name);
            var ouzeroUser = GetUser(ouzero.Username);
            var userLists = GetUserSubscribedLists(ouzeroUser);

            foreach(var tweetList in userLists)
            {
                scoredHandles.AddRange(GetScoredHandlesFromUserList(tweetList.Slug, tweetList.Creator.ScreenName));
            }

            return scoredHandles;
        }

        public static IEnumerable<TwitterHandle> GetScoredHandlesFromUserList(string listName, string listCreator)
        {
            var usersInList = GetUsersFromList(listName, listCreator);

            return GetScoredHandlesFromUsers(usersInList, "");
        }

        public static IEnumerable<IUser> GetUsersFromList(string listName, string listCreator)
        {
            var tweetList = TweetList.GetExistingList(listName, listCreator);
            var usersInList = GetUsersInList(tweetList);
            return usersInList;
        }


        private static List<TwitterHandle> GetScoredHandlesFromUsers(IEnumerable<IUser> users, string category)
        {
            var scoredHandles = new List<TwitterHandle>();
            foreach(var user in users)
            {
                if(user == null)
                    continue;

                var populatedHandle = GetPopulatedHandleFromUser(user, category);
                if(populatedHandle.Username.StartsWith("invisible"))
                {
                    MissingHandles.Add(new TwitterHandle(user.ScreenName));
                }
                else
                {
                    scoredHandles.Add(populatedHandle);
                }
            }

            return scoredHandles;
        }


        private static List<TwitterHandle> GetScoredHandlesFromUsernames(IEnumerable<string> usernames, string category)
        {
            var userList = usernames.Select(GetUser).ToList();
            return GetScoredHandlesFromUsers(userList, category);
        }

        private static IUser GetUser(string name)
        {
            return User.GetUserFromScreenName(name);
        }

        private static IEnumerable<ITweetList> GetUserSubscribedLists(IUser user)
        {
            var tweetLists = TweetList.GetUserLists(user, false);
            return tweetLists.ToArray();
        }

        private static IEnumerable<IUser> GetUsersInList(ITweetList list)
        {
            return list.GetMembers(10000);
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
            if(website == null || website.Equals(string.Empty))
                return "unspecified";
            return website;
        }

        private static TwitterHandle GetPopulatedHandleFromUser(IUser user, string category)
        {
            var h = new TwitterHandle(user.ScreenName)
            {
                Name = user.Name,
                ImgUrl = user.ProfileImageUrl,
                Followers = user.FollowersCount,
                Friends = user.FriendsCount,
                Location = GetLocation(user),
                Website = GetWebsite(user),
                Category = category
            };

            var desc = GetBio(user);
            desc = desc.Replace("\r\n", " ");
            desc = desc.Replace("\n", " ");
            desc = desc.Replace("\r", " ");
            h.Bio = desc;

            var numberOfTweets = 200;

            ITweet[] tweets;
            try
            {
                tweets = GetUserTimelineTweets(user, numberOfTweets);
            }
            catch(Exception e) //something went wrong
            {
                var errorMessage = string.Format("Error getting populated handle '{0}' : {1}", user.ScreenName, e.Message);
                Console.WriteLine(errorMessage);
                return GetInvisibleUser();
            }

            if(tweets.Length != 500) // in case the account doesn't have the required amount
            {
                if(tweets.Length == 0)
                {
                    return GetInvisibleUser();
                }
                numberOfTweets = tweets.Length;
            }

            var totalRetweets = 0;
            var totalFavourites = 0;

            foreach(var t in tweets)
            {
                if(!t.IsRetweet)
                    totalRetweets += t.RetweetCount;
                totalFavourites += t.FavouriteCount;
            }

            h.RetweetRate = (totalRetweets / numberOfTweets) + 1;
            h.FavouriteRate = (totalFavourites / numberOfTweets) + 1;

            h.Score = ComputeScoreStatic(h);

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
        private static double ComputeScoreStatic(TwitterHandle h)
        {
            var score = Math.Pow(h.RetweetRate, 2) + h.FavouriteRate + h.Friends / 100 + (h.Followers / 1000);
            return score;
        }

        /// <summary>
        /// Gets a number of tweets up to the maximum specified for a given user. Excludes retweets of other users.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="maxNumberOfTweets"></param>
        /// <returns></returns>
        private static ITweet[] GetUserTimelineTweets(IUser user, int maxNumberOfTweets)
        {
            var tweets = new List<ITweet>();

            var userTimelineParameter = Timeline.CreateUserTimelineRequestParameter(user);
            userTimelineParameter.IncludeRTS = false;
            userTimelineParameter.MaximumNumberOfTweetsToRetrieve = maxNumberOfTweets;
            var receivedTweets = Timeline.GetUserTimeline(userTimelineParameter).ToArray();

            tweets.AddRange(receivedTweets);

            while(tweets.Count < maxNumberOfTweets && receivedTweets.Length == 200)
            {
                var oldestTweet = tweets.Min(x => x.Id);
                userTimelineParameter.MaxId = oldestTweet;
                userTimelineParameter.MaximumNumberOfTweetsToRetrieve = 200;

                receivedTweets = Timeline.GetUserTimeline(userTimelineParameter).ToArray();
                tweets.AddRange(receivedTweets);
            }

            return tweets.Distinct().ToArray();
        }

        private static void PopulateKeysDictionary()
        {
            const string path = @"C:\Users\Nishant\Documents\twitterkeys.csv";
            _keys = File.ReadLines(path).Select(line => line.Split(',')).ToDictionary(line => line[0], line => line[1]);
        }

        public static List<TwitterHandle> ScoreHandlesFromFiles(string[] files, string category)
        {
            var scoredHandles = new List<TwitterHandle>();
            ScoringTimes = new List<string>();

            foreach(var file in files.Where(file => file.Contains("ToDo")))
            {
                while(true)
                {
                    SpinWait.SpinUntil(() => false, 1000);
                    if(!IsTimeToScore(DateTime.Now))
                        continue;
                    var time = @"Scored at " + DateTime.Now.ToString("G");
                    ScoringTimes.Add(time);
                    Console.WriteLine(time);
                    scoredHandles.AddRange(ScoreHandlesFromFile(file, category));
                    break;
                }
            }

            return scoredHandles;
        }



        private static IEnumerable<TwitterHandle> ScoreHandlesFromFile(string path, string category)
        {
            var reader = new CsvFileReader(path);
            var handlesFromFile = reader.GetHandlesFromFile();
            var scoredHandles = GetScoredHandlesFromUsernames(handlesFromFile, category);
            return scoredHandles;
        }

        private static bool IsTimeToScore(DateTime currentTime)
        {
            if(_lastScoreTime == DateTime.MinValue)
            {
                _lastScoreTime = DateTime.Now;
                return true;
            }

            if(!(Math.Abs(currentTime.Subtract(_lastScoreTime).TotalMinutes) > 15))
                return false;
            _lastScoreTime = DateTime.Now;
            return true;
        }
    }
}
