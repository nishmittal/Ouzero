using FluentNHibernate.Mapping;
using SentimentAnalysis.Entities;

namespace SentimentAnalysis.Mappings
{
    // ReSharper disable once UnusedMember.Global
    public class ScoredHandleMap : ClassMap<ScoredHandle>
    {
        public ScoredHandleMap()
        {
            Id(x => x.Username);
            Map(x => x.Name);
            Map(x => x.Bio).Column("TwitBio");
            Map(x => x.ImgUrl);
            Map(x => x.Followers).Column("TwitFollowers");
            Map(x => x.Friends).Column("TwitFriends");
            Map(x => x.RetweetRate).Column("TwitRetweetRate");
            Map(x => x.FavouriteRate).Column("TwitFavouriteRate");
            Map(x => x.Location).Column("TwitLocation");
            Map(x => x.Website).Column("TwitWebsite");
            Map(x => x.AlexaRank);
            Map(x => x.AlexaBounce);
            Map(x => x.AlexaPagePerf);
            Map(x => x.AlexaTraffic);
            Map(x => x.Category);
            Map(x => x.Score);

            Table("ScoredHandles");
        }
    }
}
