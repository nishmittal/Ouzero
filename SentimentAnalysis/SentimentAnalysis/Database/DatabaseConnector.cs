using System.Collections.Generic;
using System.Linq;
using SentimentAnalysis.Entities;

namespace SentimentAnalysis.Database
{
    public class DatabaseConnector
    {
        public static void InsertRecords(IEnumerable<TwitterHandle> scoredHandles)
        {
            var handles = GetScoredHandlesList(scoredHandles);
            var session = FluentNHibernateHelper.OpenStatelessSession();
            using(session)
            {
                session.SetBatchSize(handles.Count);
                foreach(var handle in handles)
                {
                    session.Insert(handle);
                }
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
    }
}