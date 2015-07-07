namespace SentimentAnalysis.Entities
{
    public class ScoredHandle
    {
        public virtual string Username { get; set; }
        public virtual string Name { get; set; }
        public virtual int Followers { get; set; }
        public virtual double RetweetRate { get; set; }
        public virtual double FavouriteRate { get; set; }
        public virtual int Friends { get; set; }
        public virtual double Score { get; set; }
        public virtual string Category { get; set; }
        public virtual string Bio { get; set; }
        public virtual string ImgUrl { get; set; }
        public virtual string Location { get; set; }
        public virtual string Website { get; set; }
        public virtual int AlexaRank { get; set; }
        public virtual int AlexaBounce { get; set; }
        public virtual int AlexaPagePerf { get; set; }
        public virtual int AlexaTraffic { get; set; }
    }
}
