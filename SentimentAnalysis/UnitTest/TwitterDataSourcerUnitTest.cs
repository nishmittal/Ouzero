using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SentimentAnalysis;
using SentimentAnalysis.Database;
using SentimentAnalysis.TwitterData;

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

        [TestMethod]
        public void ShouldGetScoredHandlesFromFileInput()
        {
            const string category = "Tech";
            var files = Directory.GetFiles("C:/Users/Nishant/Desktop/Dropbox/Ouzero/tech-news-people/");
            var scoredHandles = TwitterDataSourcer.ScoreHandlesFromFiles(files, category);
            DatabaseConnector.InsertRecords(scoredHandles);
            Utilities.WriteMissingHandlesFile(category);
            var scoringTimes = TwitterDataSourcer.ScoringTimes;
        }

        [TestMethod, Ignore]
        public void ShouldGetMyScoredHandles()
        {
            const string creator = "nandita";
            const string listName = "food-bloggers";
            const string category = "Food";
            TwitterDataSourcer.SetCredentials();
            var scoredHandles = TwitterDataSourcer.GetScoredHandlesFromUserList(listName, creator);
            // Write sample data to CSV file
            Utilities.WriteFiles(scoredHandles, category);
        }

    }
}
