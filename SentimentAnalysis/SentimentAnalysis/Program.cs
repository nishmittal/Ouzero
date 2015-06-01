using System;
using System.Collections.Generic;

namespace SentimentAnalysis
{
    class Program
    {
        private IList<TwitterHandle> _handles;

        static void Main()
        {
            Go();
            Console.ReadLine();
        }

        public Program()
        {
            _handles = new List<TwitterHandle> { new TwitterHandle( "@Ouzer0" ) };
        }

        private static void Go()
        {
            TwitterDataSourcer.SetCredentials();
            var scoredHandles = TwitterDataSourcer.GetScoredHandlesFromUserLists( "Ouzer0" );
            // Write sample data to CSV file
            using ( var writer = new CsvFileWriter( "C:/Users/Nishant/Desktop/Dropbox/Docs/scores.csv" ) )
                foreach ( var h in scoredHandles )
                {
                    var row = new CsvRow { h.Username, h.Followers.ToString(), ( (int) h.RetweetRate ).ToString(), ( (int) h.FavouriteRate ).ToString(), h.Friends.ToString(), h.Category, ( (int) h.Score ).ToString() };
                    writer.WriteRow( row );
                }
        }
    }
}
