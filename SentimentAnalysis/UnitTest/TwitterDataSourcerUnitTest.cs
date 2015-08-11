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
        public void ShouldGetScoredHandlesFromFileInput()
        {
            const string category = "Fitness";
            var path = @"C:\Users\Nishant\Desktop\Dropbox\Ouzero\Fitness-people\";
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

        [TestMethod, Ignore]
        public void ShouldCreateFilesOfHandlesFromList()
        {
            const string creator = "ProtectCELL";
            const string listName = "tech-moms";
            const string category = "Tech";
            Utilities.SplitTwitterListIntoHandleChunks(creator, listName, category);
        }
    }
}
