namespace SentimentAnalysis
{
    /// <summary>
    /// Represents a Twitter Handle for which we store data.
    /// </summary>
    public class TwitterHandle
    {
        /// <summary>
        /// Twitter username.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Name of account.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Number of followers.
        /// </summary>
        public int Followers { get; set; }
        /// <summary>
        /// Retweets per tweet.
        /// </summary>
        public double RetweetRate { get; set; }
        /// <summary>
        /// Favourites per tweet.
        /// </summary>
        public double FavouriteRate { get; set; }
        /// <summary>
        /// Number of friends.
        /// </summary>
        public int Friends { get; set; }
        /// <summary>
        /// Ouzero score.
        /// </summary>
        public double Score { get; set; }
        /// <summary>
        /// Category of user. (e.g. tech, food, cars)
        /// </summary>
        public string Category { get; set;}

        public string Bio { get; set; }
        public string ImgUrl { get; set; }
        public string Location { get; set; }
        public string Website { get; set; }
        public int AlexaRank { get; set; }
        public int AlexaBounce { get; set; }
        public int AlexaPagePer { get; set; }
        public int AlexaTraffic { get; set; }

        /// <summary>
        /// Constructor for the TwitterHandle object.
        /// </summary>
        /// <param name="username">Twitter username of the account represented by this object.</param>
        public TwitterHandle( string username )
        {
            Username = username;
            Category = "unsorted";
        }
    }
}
