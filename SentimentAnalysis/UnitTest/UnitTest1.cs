using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SentimentAnalysis.Interfaces;
using SentimentAnalysis;
using System.Collections.Generic;
using Tweetinvi.Core.Interfaces;
using Tweetinvi;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestMethod1()
        {
            IDataSourcer tds;
            IList<TwitterHandle> Handles;

            Handles = new List<TwitterHandle>();
            TwitterHandle h = new TwitterHandle( "@katyperry" );
            Handles.Add( h );
            tds = new TwitterDataSourcer( Handles );
            h = TwitterDataSourcer.PopulateWithData( h );
            var numberOfTweets = 200;
            var tweets = TwitterDataSourcer.GetUserTimelineTweets( h.Name, numberOfTweets );

            if ( tweets.Length != 500 ) // in case the account doesn't have the required amount
            {
                numberOfTweets = tweets.Length;
            }

            var totalRetweets = 1;
            var totalFavourites = 1;

            foreach ( ITweet t in tweets )
            {
                totalRetweets += t.RetweetCount;
                totalFavourites += t.FavouriteCount;
            }

            h.RetweetRate = (totalRetweets / numberOfTweets) + 1;
            h.FavouriteRate = (totalFavourites / numberOfTweets) + 1;

            var score = TwitterDataSourcer.ComputeScoreStatic( h );

            Assert.AreEqual( tweets.Length, numberOfTweets );


        }
    }
}
