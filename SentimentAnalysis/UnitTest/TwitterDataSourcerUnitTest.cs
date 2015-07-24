using Microsoft.VisualStudio.TestTools.UnitTesting;
using SentimentAnalysis;
using SentimentAnalysis.Csv;
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

        [TestMethod, Ignore]
        public void ShouldGetScoredHandlesFromFileInput()
        {
            const string filename = "Tech-ToDo-2";
            const string path = "C:/Users/Nishant/Desktop/Dropbox/Ouzero/tech-news-people/" + filename + ".csv";
            const string category = "Tech";
            ScoreHandlesFromFile(path, category);
        }

        private static void ScoreHandlesFromFile(string path, string category)
        {
            var reader = new CsvFileReader(path);
            var handlesFromFile = reader.GetHandlesFromFile();
            var scoredHandles = TwitterDataSourcer.GetScoredHandlesFromUsernames(handlesFromFile);
            Utilities.WriteFiles(scoredHandles, category);
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
