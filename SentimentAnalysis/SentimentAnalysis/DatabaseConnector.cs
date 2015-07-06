using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SentimentAnalysis.Properties;

namespace SentimentAnalysis
{
    class DatabaseConnector
    {
        private readonly SqlConnection _connection;

        public DatabaseConnector()
        {
            _connection = new SqlConnection(Settings.Default.DbConnectionString);
            
        }

        public void ConnectToDatabase()
        {
            _connection.Open();
        }

        public void CloseConnection()
        {
            _connection.Close();
            _connection.Dispose();
        }

        public void InsertScoredHandle(TwitterHandle handle)
        {
            using (_connection)
            {
                var command = new SqlCommand("INSERT INTO ScoredHandles " +
                                             "VALUES" +
                                             "@Handle" +
                                             "@img" +
                                             "@twitfollow" +
                                             "@twitfriend" +
                                             "@twitretweet" +
                                             "@twitfav" +
                                             "@twitbio" +
                                             "@twitwebsite" +
                                             "@alexarank" +
                                             "@alexabounce" +
                                             "@alexapageperf" +
                                             "@alexatraffic" +
                                             "@Category" +
                                             "@score", _connection);

                command.Parameters.Add(new SqlParameter("Handle", handle.Username));
                command.Parameters.Add( new SqlParameter( "img", handle.ImgUrl ) );
                command.Parameters.Add( new SqlParameter( "twitfollow", handle.Followers ) );
                command.Parameters.Add( new SqlParameter( "twitfriend", handle.Friends ) );
                command.Parameters.Add( new SqlParameter( "twitretweet", handle.RetweetRate ) );
                command.Parameters.Add( new SqlParameter( "twitfav", handle.FavouriteRate ) );
                command.Parameters.Add( new SqlParameter( "twitbio", handle.Bio ) );
                command.Parameters.Add( new SqlParameter( "twitwebsite", handle.Website ) );
                command.Parameters.Add( new SqlParameter( "alexarank", handle.AlexaRank ) );
                command.Parameters.Add( new SqlParameter( "alexabounce", handle.AlexaBounce ) );
                command.Parameters.Add( new SqlParameter( "alexapageperf", handle.AlexaPagePer ) );
                command.Parameters.Add( new SqlParameter( "alexatraffic", handle.AlexaTraffic ) );
                command.Parameters.Add( new SqlParameter( "Category", handle.Category ) );
                command.Parameters.Add( new SqlParameter( "score", handle.Score ) );

                Console.WriteLine( "Commands executed! Total rows affected are " + command.ExecuteNonQuery() );
            }
        }
    }
}
