using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SentimentAnalysis;

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
            const string path = "C:/Users/Nishant/Desktop/Dropbox/Ouzero/leftover-food.csv";
            var reader = new CsvFileReader( path );
            var handlesFromFile = reader.GetHandlesFromFile();
            var scoredHandles = TwitterDataSourcer.GetScoredHandlesFromUsernames( handlesFromFile );
            WriteFiles( scoredHandles, "food" );

        }


        [TestMethod]
        public void ShouldGetMyScoredHandles()
        {
            TwitterDataSourcer.SetCredentials();
            var creator = "nandita";
            var listName = "food-bloggers";
            var category = "Food";
            var scoredHandles = TwitterDataSourcer.GetScoredHandlesFromUserList( listName, creator );
            // Write sample data to CSV file
            WriteFiles( scoredHandles, category );
        }

        private static void WriteFiles( IEnumerable<TwitterHandle> scoredHandles, string category )
        {
            var path = "C:/Users/Nishant/Desktop/Dropbox/Ouzero/" + category + ".csv";
            if ( File.Exists( path ) )
            {
                path = "C:/Users/Nishant/Desktop/Dropbox/Ouzero/" + category + "1.csv";
            }
            using ( var writer = new CsvFileWriter( path ) )
                foreach ( var h in scoredHandles )
                {
                    var row = new CsvRow
                    {
                        h.Name,
                        h.Followers.ToString(),
                        ((int) h.RetweetRate).ToString(),
                        ((int) h.FavouriteRate).ToString(),
                        h.Friends.ToString(),
                        category,
                        ((int) h.Score).ToString(),
                        h.Bio,
                        h.Location
                    };
                    writer.WriteRow( row );
                }

            path = "C:/Users/Nishant/Desktop/Dropbox/Ouzero/leftover-" + category + ".csv";
            if ( File.Exists( path ) )
            {
                path = "C:/Users/Nishant/Desktop/Dropbox/Ouzero/leftover-" + category + "1.csv";
            }
            var missingHandles = TwitterDataSourcer.MissingHandles;
            using ( var writer = new CsvFileWriter( path ) )
                foreach ( var h in missingHandles )
                {
                    var row = new CsvRow { h.Name };
                    writer.WriteRow( row );
                }
        }
    }
}
