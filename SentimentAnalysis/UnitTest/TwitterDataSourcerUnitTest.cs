using Microsoft.VisualStudio.TestTools.UnitTesting;
using SentimentAnalysis;
using Tweetinvi;
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
            var t = TwitterDataSourcer.GetUserTimelineTweets(TwitterDataSourcer.GetUser("gamehead"), 200);
            var num = t.Length;
        }


        [TestMethod]
        public void ShouldGetMyScoredHandles()
        {
            TwitterDataSourcer.SetCredentials();
            var creator = "Scobleizer";
            var listName = "most-influential-in-tech";
            var category = "Tech";
            var scoredHandles = TwitterDataSourcer.GetScoredHandlesFromUserList( listName, creator );
            // Write sample data to CSV file
            using ( var writer = new CsvFileWriter( "C:/Users/Nishant/Desktop/Dropbox/Ouzero/tech.csv" ) )
                foreach ( var h in scoredHandles )
                {
                    var row = new CsvRow { h.Name, h.Followers.ToString(), ( (int) h.RetweetRate ).ToString(), ( (int) h.FavouriteRate ).ToString(), h.Friends.ToString(), category, ( (int) h.Score ).ToString(), h.Bio, h.Location };
                    writer.WriteRow( row );
                }

            var missingHandles = TwitterDataSourcer.MissingHandles;
            using ( var writer = new CsvFileWriter( "C:/Users/Nishant/Desktop/Dropbox/Ouzero/missing.csv" ) )
                foreach ( var h in missingHandles )
                {
                    var row = new CsvRow { h.Name };
                    writer.WriteRow( row );
                }
        }

    }
}
