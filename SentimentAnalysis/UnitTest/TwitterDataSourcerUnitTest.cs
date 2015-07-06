using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SentimentAnalysis;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces;

namespace UnitTest
{
    [TestClass]
    public class TwitterDataSourcerUnitTest
    {
        [TestInitialize]
        public void Setup()
        {
            TwitterDataSourcer.SetCredentials();
        }

        //TODO Need to write these tests

        [TestMethod]
        public void ShouldSetUpCredentialsCorrectly()
        {

        }

        [TestMethod]
        public void ShouldGetUserFromName()
        {

        }

        [TestMethod]
        public void ShouldGetTheCorrectNumberOfSubscribedLists()
        {

        }

        [TestMethod]
        public void ShouldGetCorrectNumberOfUsersInList()
        {

        }

        [TestMethod]
        public void ShouldSetUpAPopulatedHandleFromAUser()
        {

        }

        [TestMethod]
        public void ShouldComputeScoreCorrectlyWhenGivenATwitterHandle()
        {

        }

        [TestMethod]
        public void ShouldGetCorrectNumberOfTweetsFromUser()
        {

        }

        [TestMethod]
        public void Blah()
        {
            
        }

        [TestMethod]
        public void ShouldGetScoredHandlesFromFileInput()
        {
            const string filename = "Tech-ToDo-2";
            const string path = "C:/Users/Nishant/Desktop/Dropbox/Ouzero/tech-news-people/" + filename + ".csv";
            const string category = "Tech";
            var reader = new CsvFileReader( path );
            var handlesFromFile = reader.GetHandlesFromFile();
            var scoredHandles = TwitterDataSourcer.GetScoredHandlesFromUsernames( handlesFromFile );
            WriteFiles( scoredHandles, category );
        }

        [TestMethod]
        public void SplitHandlesIntoChunks()
        {
            const int listSize = 88;
            const string creator = "Scobleizer";
            const string listName = "tech-news-people";
            const string category = "Tech";

            TwitterDataSourcer.SetCredentials();

            var usersFromList = TwitterDataSourcer.GetUsersFromList(listName, creator) as IList<IUser>;

            if (usersFromList == null) return;

            var names = usersFromList.Select(user => user.ScreenName).ToList();
            var chunks = SplitList( names, listSize );

            var index = 0;

            foreach (var list in chunks)
            {
                var path = "C:/Users/Nishant/Desktop/Dropbox/Ouzero/" + category + "-ToDo-" + index + ".csv";
                using ( var writer = new CsvFileWriter( path ) )
                    foreach ( var h in list )
                    {
                        var row = new CsvRow{h};
                        writer.WriteRow( row );
                    }

                index++;
            }
        }

        public static List<List<string>> SplitList( List<string> names, int nSize = 88 )
        {
            var list = new List<List<string>>();

            for ( var i = 0; i < names.Count; i += nSize )
            {
                list.Add( names.GetRange( i, Math.Min( nSize, names.Count - i ) ) );
            }

            return list;
        } 

        [TestMethod]
        public void ShouldGetMyScoredHandles()
        {
            const string creator = "nandita";
            const string listName = "food-bloggers";
            const string category = "Food";
            TwitterDataSourcer.SetCredentials();
            var scoredHandles = TwitterDataSourcer.GetScoredHandlesFromUserList( listName, creator );
            // Write sample data to CSV file
            WriteFiles( scoredHandles, category );
        }

        private static void WriteFiles( IEnumerable<TwitterHandle> scoredHandles, string category )
        {
            var path = "C:/Users/Nishant/Desktop/Dropbox/Ouzero/tech-news-people/" + category + ".csv";
            if ( File.Exists( path ) )
            {
                path = "C:/Users/Nishant/Desktop/Dropbox/Ouzero/tech-news-people/" + category + "1.csv";
            }
            using ( var writer = new CsvFileWriter( path ) )
                foreach ( var h in scoredHandles )
                {
                    var row = new CsvRow
                    {
                        h.Username,
                        h.ImgUrl,
                        h.Followers.ToString(),
                        h.Friends.ToString(),
                        ((int) h.RetweetRate).ToString(),
                        ((int) h.FavouriteRate).ToString(),
                        h.Bio,
                        h.Website,
                        h.AlexaRank.ToString(),
                        h.AlexaBounce.ToString(),
                        h.AlexaPagePer.ToString(),
                        h.AlexaTraffic.ToString(),
                        category,
                        ((int) h.Score).ToString()
                        //h.Location
                    };

                    writer.WriteRow( row );
                }

            var missingHandles = TwitterDataSourcer.MissingHandles;

            if (missingHandles.IsNullOrEmpty())
                return;

            path = "C:/Users/Nishant/Desktop/Dropbox/Ouzero/leftover-" + category + ".csv";
            if ( File.Exists( path ) )
            {
                path = "C:/Users/Nishant/Desktop/Dropbox/Ouzero/leftover-" + category + "1.csv";
            }
            using ( var writer = new CsvFileWriter( path ) )
                foreach ( var h in missingHandles )
                {
                    var row = new CsvRow { h.Username };
                    writer.WriteRow( row );
                }
        }
    }
}
