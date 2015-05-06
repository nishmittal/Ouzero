using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SentimentAnalysis;
using Tweetinvi.Core.Interfaces;

namespace UnitTest
{
    [TestClass]
    public class TwitterDataSourcerUnitTest
    {
        private TwitterDataSourcer tds;
        private IList<TwitterHandle> Handles;
        [TestInitialize]
        public void Setup()
        {
            Handles = new List<TwitterHandle> {new TwitterHandle("@Ouzer0")};
            tds = new TwitterDataSourcer(Handles);
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

    }
}
