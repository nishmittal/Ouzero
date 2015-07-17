using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SentimentAnalysis.Csv;

namespace UnitTest
{
    [TestClass]
    public class CsvFileReaderUnitTest
    {
        [TestMethod]
        public void ShouldReadFortyLinesFromFile()
        {
            const string path = @"testing\csvtest.csv";
            var reader = new CsvFileReader(path);
            var handles = reader.GetHandlesFromFile();
            Assert.AreEqual(40, handles.Count);
        }

        [TestMethod]
        public void ShouldConvertCsvToDictionary()
        {
            const string path = @"C:\Users\Nishant\Documents\twitterkeys.csv";
            var dict = File.ReadLines(path).Select(line => line.Split(',')).ToDictionary(line => line[0], line => line[1]);
        }
    }
}
