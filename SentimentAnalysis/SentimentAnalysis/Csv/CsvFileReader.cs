using System.Collections.Generic;
using System.IO;

namespace SentimentAnalysis.Csv
{
    public class CsvFileReader : StreamReader
    {
        public CsvFileReader( Stream stream )
            : base( stream )
        {
        }

        public CsvFileReader( string filename )
            : base( filename )
        {
        }

        public List<string> GetHandlesFromFile()
        {
            var handles = new List<string>();

            while ( !EndOfStream )
            {
                var line = ReadLine();
                if ( !string.IsNullOrEmpty( line ) )
                {
                    handles.Add( line );
                }
            }

            return handles;
        }
    }
}
