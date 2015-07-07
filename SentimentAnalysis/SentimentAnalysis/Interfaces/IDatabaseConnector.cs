using System.Collections.Generic;

namespace SentimentAnalysis
{
    public interface IDatabaseConnector
    {
        void ConnectToDatabase();
        void CloseConnection();
        void InsertScoredHandle( TwitterHandle handle );
        IList<TwitterHandle> GetAllRecords();
    }
}