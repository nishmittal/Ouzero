using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SentimentAnalysis.Csv
{

    /// <summary>
    /// Class to store one CSV row
    /// </summary>
    public class CsvRow : List<string>
    {
        public string LineText { get; set; }
    }

    public class CsvFileWriter : StreamWriter
    {
        public CsvFileWriter(string filename)
            : base(filename)
        {
        }

        /// <summary>
        /// Writes a single row to a CSV file.
        /// </summary>
        /// <param name="row">The row to be written</param>
        public void WriteRow(CsvRow row)
        {
            var builder = new StringBuilder();
            var firstColumn = true;
            foreach(var value in row)
            {
                // Add separator if this isn't the first value
                if(!firstColumn)
                    builder.Append(',');
                // Implement special handling for values that contain comma or quote
                // Enclose in quotes and double up any double quotes
                try
                {

                    if(value.IndexOfAny(new[] { '"', ',' }) != -1)
                        builder.AppendFormat("\"{0}\"", value.Replace("\"", "\"\""));
                    else
                        builder.Append(value);
                }
                catch(Exception e)
                {
                    Console.WriteLine(@"Error when writing file: " + e.Message);
                }

                firstColumn = false;
            }
            row.LineText = builder.ToString();
            WriteLine(row.LineText);
        }
    }
}
