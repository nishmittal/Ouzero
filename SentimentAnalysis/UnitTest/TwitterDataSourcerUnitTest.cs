using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SentimentAnalysis;
using SentimentAnalysis.Database;
using SentimentAnalysis.TwitterData;
using Tweetinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Credentials;

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

        [TestMethod, Ignore]
        public void ShouldGetScoredHandlesFromFileInput()
        {
            const string category = "Travel";
            var path = @"C:\Users\Nishant\Desktop\Dropbox\Ouzero\Tech-moms\";
            var files = Directory.GetFiles(path);
            var scoredHandles = TwitterDataSourcer.ScoreHandlesFromFiles(files, category);
            Utilities.WriteScoredHandlesFile(path, scoredHandles, category);
            DatabaseConnector.BatchInsertRecords(scoredHandles);
            Utilities.WriteMissingHandlesFile(path, category);
            var scoringTimes = TwitterDataSourcer.ScoringTimes;
            path = path + "ScoringTimes.txt";
            Utilities.WriteFile(path, scoringTimes);
        }

        [TestMethod, Ignore]
        public void ShouldCreateFilesOfHandlesFromList()
        {
            const string creator = "EarthPower_D";
            const string listName = "art";
            const string category = "Test";
            var existingList = TwitterList.GetExistingList(listName, creator);
            var x = existingList.MemberCount;
        }

        [TestMethod]
        public void TestGetScoredHandlesFromTwitterLists()
        {
            var listUrls = new List<string>
            {
                "https://twitter.com/EarthPower_D/lists/art",
                "https://twitter.com/BloombergTV/lists/autos",
                "https://twitter.com/EarthPower_D/lists/food-wellness",
                "https://twitter.com/EarthPower_D/lists/business",
                "https://twitter.com/EarthPower_D/lists/lifestyle",
                "https://twitter.com/EarthPower_D/lists/social-media"
            };
            TwitterDataSourcer.GetScoredHandlesFromTwitterLists(listUrls);
        }


        // read in a text file of list urls
        // create lists from these urls
        // get members of list and infer category from url
        // get list of already scored handles from database
        // if rate limit not hit then
        // score members not existing in database periodically
        // chuck it into database and to csv export
        // else if rate limit hit, wait
    }
}
