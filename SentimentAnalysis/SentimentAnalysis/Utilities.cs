﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SentimentAnalysis.Csv;
using SentimentAnalysis.Entities;
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
                var path = "C:/Users/Nishant/Desktop/Dropbox/Ouzero/" + listName + "/" + category + "-ToDo-" + index + ".csv";
                WriteFile(path, list);
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

        public static void WriteScoredHandlesFile(string folderPath, IEnumerable<TwitterHandle> scoredHandles, string category)
        {
            if(folderPath.EndsWith(@"\"))
            {
                folderPath = folderPath + @"\";
            }
            var path = folderPath + category + ".csv";
            if(File.Exists(path))
            {
                path = folderPath + category + "1.csv";
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
                    h.AlexaRank.ToString(),
                    h.AlexaBounce.ToString(),
                    h.AlexaPagePer.ToString(),
                    h.AlexaTraffic.ToString(),
                    category,
                    ((int) h.Score).ToString(),
                    h.Bio,
                    h.Website
                    //h.Location
                    //h.Name
                }))
                {
                    writer.WriteRow(row);
                }
        }

        public static void WriteMissingHandlesFile(string folderPath, string category)
        {
            var missingHandles = TwitterDataSourcer.MissingHandles;

            if(missingHandles.IsNullOrEmpty())
                return;

            var path = folderPath + "leftover-" + category + ".csv";
            if(File.Exists(path))
            {
                path = folderPath + "/leftover-" + category + "_1.csv";
            }

            var lines = missingHandles.Select(h => h.Username);

            WriteFile(path, lines);
        }


        public static void WriteFile(string path, IEnumerable<string> lines)
        {
            if(!File.Exists(path))
            {
                var directory = (new FileInfo(path)).Directory;
                if(directory != null)
                    directory.Create();
            }
            using(var writer = new CsvFileWriter(path))
                foreach(var line in lines.Select(l => new CsvRow { l }))
                {
                    writer.WriteRow(line);
                }
        }

        public static void WriteDatabaseExportFile(string path, List<ScoredHandle> records)
        {
            using(var writer = new CsvFileWriter(path))
                foreach(var row in records.Select(h => new CsvRow
                {
                    h.Username,
                    h.ImgUrl,
                    h.Followers.ToString(),
                    h.Friends.ToString(),
                    ((int) h.RetweetRate).ToString(),
                    ((int) h.FavouriteRate).ToString(),
                    h.AlexaRank.ToString(),
                    h.AlexaBounce.ToString(),
                    h.AlexaPagePerf.ToString(),
                    h.AlexaTraffic.ToString(),
                    h.Category,
                    ((int) h.Score).ToString(),
                    h.Bio,
                    h.Website
                    //h.Location
                    //h.Name
                }))
                {
                    writer.WriteRow(row);
                }
        }
    }
}
