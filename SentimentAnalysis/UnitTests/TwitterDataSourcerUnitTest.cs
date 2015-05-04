using System;
using NUnit.Framework;
using SentimentAnalysis.Interfaces;
using System.Collections.Generic;
using SentimentAnalysis;


namespace UnitTests
{
    [TestFixture]
    public class TwitterDataSourcerUnitTest
    {
        IDataSourcer tds;
        IList<TwitterHandle> Handles;

        [TestFixtureSetUp]
        public void Setup()
        {
            Handles = new List<TwitterHandle>();
            Handles.Add( new TwitterHandle( "@NetshockTech" ) );
            tds = new TwitterDataSourcer( Handles );
        }


        [Test]
        public void ShouldGetLastTenTweets()
        {
            var numberOfTweetsToGet = 10;
            var tweets = TwitterDataSourcer.GetUserTimelineTweets( "@NetshockTech", numberOfTweetsToGet );
            Assert.AreEqual( tweets.Length, numberOfTweetsToGet );
        }
    }
}
