using System.IO;
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
        public void QueryRateLimits()
        {
            var rateLimits = TwitterDataSourcer.GetRateLimits();
        }

        [TestMethod]
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
        public void ShouldGetMyScoredHandles()
        {
            const string creator = "nandita";
            const string listName = "food-bloggers";
            const string category = "Food";
            TwitterDataSourcer.SetCredentials();
            var scoredHandles = TwitterDataSourcer.GetScoredHandlesFromUserList(listName, creator);
            // Write sample data to CSV file
            var mainFolderPath = @"C:\Users\Nishant\Desktop\Dropbox\Ouzero\";
            Utilities.WriteScoredHandlesFile(mainFolderPath, scoredHandles, category);
        }

        [TestMethod]
        public void ShouldCreateFilesOfHandlesFromList()
        {
            const string creator = "verified";
            const string listName = "world-leaders";
            const string category = "Test";
            Utilities.SplitTwitterListIntoHandleChunks(creator, listName, category);
        }

        [TestMethod]
        public void Gash()
        {
            var url = "https://twitter.com/verified/lists/world-leaders";
            var strings = url.Split('/');
            string creator = strings[3];
            string listName = strings[5];
            string category = listName.Replace('-', ' ').ToUpper();

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
