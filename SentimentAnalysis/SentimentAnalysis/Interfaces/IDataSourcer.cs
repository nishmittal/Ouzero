namespace SentimentAnalysis.Interfaces
{
    /// <summary>
    /// Interface for implementations which will source data from social media sites.
    /// </summary>
    public interface IDataSourcer
    {
        void GetData();

        int ComputeScore(TwitterHandle h);
    }
}
