using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SentimentAnalysis;

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
    }
}
