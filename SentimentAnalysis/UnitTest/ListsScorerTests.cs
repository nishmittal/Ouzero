using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SentimentAnalysis;

namespace UnitTest
{
    [TestClass]
    public class ListsScorerTests
    {
        private const string VerifiedListMedia = "https://twitter.com/verified/lists/media";

        [TestInitialize]
        public void Setup()
        {
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Score_EmptyList_ThrowsArgumentException()
        {
            ListsScorer.Score(new List<string>());
        }

        [TestMethod]
        public void Score_ListWithOneUrl_CreatesOneTwitterList()
        {
            var twitterLists = new List<string> { VerifiedListMedia };

            ListsScorer.Score(twitterLists);

            Assert.AreEqual(1, ListsScorer.TwitterLists.Count);
        }

        
    }
}