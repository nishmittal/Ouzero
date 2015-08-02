using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SentimentAnalysis;

namespace UnitTest
{
    [TestClass]
    public class UtilitiesTests
    {
        [TestMethod]
        public void WriteFile_ListOfOneLine_ShouldWriteOneLineFileToPath()
        {
            var lines = new List<string> { "1" };
            const string path = @"testing\writerTest.csv";

            Utilities.WriteFile(path, lines);

            Assert.IsTrue(File.Exists(path));
            Assert.AreEqual(lines.Count, File.ReadLines(path).Count());
        }
    }
}