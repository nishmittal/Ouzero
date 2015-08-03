using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using NHibernate.Linq;
using SentimentAnalysis.Entities;
using SentimentAnalysis.TwitterData;

namespace SentimentAnalysis.Database
{
    public static class DatabaseConnector
    {
        public static void BatchInsertRecords(IEnumerable<TwitterHandle> scoredHandles)
        {
            var handles = GetScoredHandlesList(scoredHandles);
            var session = FluentNHibernateHelper.OpenStatelessSession();
            using(session)
            {
                var list = session.Query<ScoredHandle>().ToList();
                Console.WriteLine(@"Records before insert: " + list.Count);
                session.SetBatchSize(handles.Count);
                foreach(var handle in handles)
                {
                    session.Insert(handle);
                }
                list = session.Query<ScoredHandle>().ToList();
                Console.WriteLine(@"Records after insert: " + list.Count);
            }
        }

        private static IList<ScoredHandle> GetScoredHandlesList(IEnumerable<TwitterHandle> twitterHandles)
        {
            return twitterHandles.Select(t => new ScoredHandle
            {
                Username = t.Username,
                Name = t.Name,
                Followers = t.Followers,
                FavouriteRate = t.FavouriteRate,
                Friends = t.Friends,
                RetweetRate = t.RetweetRate,
                ImgUrl = t.ImgUrl,
                Score = t.Score,
                Website = t.Website,
                Category = t.Category,
                Location = t.Location,
                AlexaPagePerf = t.AlexaPagePer,
                AlexaBounce = t.AlexaBounce,
                AlexaTraffic = t.AlexaTraffic,
                AlexaRank = t.AlexaRank,
                Bio = t.Bio
            }).ToList();
        }

        public static int GetNumberOfRows()
        {
            var session = FluentNHibernateHelper.OpenSession();
            return session.QueryOver<ScoredHandle>()
                .Select(Projections.RowCount())
                .FutureValue<int>()
                .Value;
        }
    }
}