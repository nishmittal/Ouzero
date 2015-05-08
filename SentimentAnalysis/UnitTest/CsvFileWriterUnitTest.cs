using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SentimentAnalysis;

namespace UnitTest
{
    [TestClass]
    public class CsvFileWriterUnitTest
    {
        [TestMethod]
        public void ShouldWriteToFile()
        {
            // Write sample data to CSV file
            using ( var writer = new CsvFileWriter( "C:/Users/Nishant/Desktop/Dropbox/Docs/scores.csv" ) )
            {
                for ( var i = 0; i < 10; i++ )
                {
                    var row = new CsvRow {"supern15h", "1", "2", "3", "4", "blah", "5"};
                    writer.WriteRow( row );
                }
            }
        }
    }
}
