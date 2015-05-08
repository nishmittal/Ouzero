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
        public string Category { get; set; }

        /// <summary>
        /// Constructor for the TwitterHandle object.
        /// </summary>
        /// <param name="name">Twitter username of the account represented by this object.</param>
        public TwitterHandle( string name )
        {
            Name = name;
            Category = "unsorted";
        }
    }
}
