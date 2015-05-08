using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SentimentAnalysis;
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
        public void ShouldGetMyScoredHandles()
        {
            TwitterDataSourcer.SetCredentials();
            var scoredHandles = TwitterDataSourcer.GetScoredHandlesFromUserLists( "Ouzer0" );
            // Write sample data to CSV file
            using ( var writer = new CsvFileWriter( "C:/Users/Nishant/Desktop/Dropbox/Docs/scores.csv" ) )
                foreach ( var h in scoredHandles )
                {
                    var row = new CsvRow { h.Name, h.Followers.ToString(), ( (int) h.RetweetRate ).ToString(), ( (int) h.FavouriteRate ).ToString(), h.Friends.ToString(), h.Category, ( (int) h.Score ).ToString() };
                    writer.WriteRow( row );
                }
        }

    }
}
