using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SentimentAnalysis.Csv;
using SentimentAnalysis.TwitterData;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces;

namespace SentimentAnalysis
{
    public static class Utilities
    {

        public static void SplitTwitterListIntoHandleChunks(string creator, string listName, string category, int listSize = 88)
        {

            TwitterDataSourcer.SetCredentials();

            var usersFromList = TwitterDataSourcer.GetUsersFromList(listName, creator) as IList<IUser>;

            if(usersFromList == null)
                return;

            var names = usersFromList.Select(user => user.ScreenName).ToList();
            var chunks = SplitList(names, listSize);

            var index = 0;

            foreach(var list in chunks)
            {
                var path = "C:/Users/Nishant/Desktop/Dropbox/Ouzero/" + category + "-ToDo-" + index + ".csv";
                using(var writer = new CsvFileWriter(path))
                    foreach(var row in list.Select(h => new CsvRow { h }))
                    {
                        writer.WriteRow(row);
                    }
                index++;
            }
        }



        private static List<List<string>> SplitList(List<string> names, int nSize = 88)
        {
            var list = new List<List<string>>();

            for(var i = 0; i < names.Count; i += nSize)
            {
                list.Add(names.GetRange(i, Math.Min(nSize, names.Count - i)));
            }

            return list;
        }

        public static void WriteScoredHandlesFile(IEnumerable<TwitterHandle> scoredHandles, string category)
        {
            var path = "C:/Users/Nishant/Desktop/Dropbox/Ouzero/tech-news-people/" + category + ".csv";
            if(File.Exists(path))
            {
                path = "C:/Users/Nishant/Desktop/Dropbox/Ouzero/tech-news-people/" + category + "1.csv";
            }
            using(var writer = new CsvFileWriter(path))
                foreach(var row in scoredHandles.Select(h => new CsvRow
                {
                    h.Username,
                    h.ImgUrl,
                    h.Followers.ToString(),
                    h.Friends.ToString(),
                    ((int) h.RetweetRate).ToString(),
                    ((int) h.FavouriteRate).ToString(),
                    h.Bio,
                    h.Website,
                    h.AlexaRank.ToString(),
                    h.AlexaBounce.ToString(),
                    h.AlexaPagePer.ToString(),
                    h.AlexaTraffic.ToString(),
                    category,
                    ((int) h.Score).ToString()
                    //h.Location
                }))
                {
                    writer.WriteRow(row);
                }

            WriteMissingHandlesFile(category);
        }

        public static void WriteMissingHandlesFile(string category)
        {
            var missingHandles = TwitterDataSourcer.MissingHandles;

            if(missingHandles.IsNullOrEmpty())
                return;

            var path = "C:/Users/Nishant/Desktop/Dropbox/Ouzero/leftover-" + category + ".csv";
            if(File.Exists(path))
            {
                path = "C:/Users/Nishant/Desktop/Dropbox/Ouzero/leftover-" + category + "_1.csv";
            }

            var lines = missingHandles.Select(h => h.Username);

            WriteFile(path, lines);
        }

        public static void WriteFile(string path, IEnumerable<string> lines)
        {
            using(var writer = new CsvFileWriter(path))
                foreach(var line in lines.Select(l => new CsvRow { l }))
                {
                    writer.WriteRow(line);
                }
        }
    }
}
