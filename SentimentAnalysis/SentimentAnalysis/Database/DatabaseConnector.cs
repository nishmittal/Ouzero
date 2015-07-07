using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SentimentAnalysis.Database;
using SentimentAnalysis.Entities;
using SentimentAnalysis.Properties;

namespace SentimentAnalysis
{
    public class DatabaseConnector : IDatabaseConnector
    {
        private readonly SqlConnection _connection;
        private readonly string _connectionString;

        public DatabaseConnector( SqlConnection connection )
        {
            _connection = connection;
            _connectionString = connection.ConnectionString;
        }

        public DatabaseConnector()
        {
            
        }

        public void ConnectToDatabase()
        {
            _connection.ConnectionString = _connectionString;
            _connection.Open();
        }

        public void CloseConnection()
        {
            _connection.Close();
            _connection.Dispose();
        }

        public void InsertScoredHandle( TwitterHandle handle )
        {
            using ( _connection )
            {
                var command = new SqlCommand( "INSERT INTO ScoredHandles " +
                                             "VALUES (" +
                                             "@Handle," +
                                             "@img," +
                                             "@twitfollow," +
                                             "@twitfriend," +
                                             "@twitretweet," +
                                             "@twitfav," +
                                             "@twitbio," +
                                             "@twitwebsite," +
                                             "@alexarank," +
                                             "@alexabounce," +
                                             "@alexapageperf," +
                                             "@alexatraffic," +
                                             "@Category," +
                                             "@score)", _connection );

                command.Parameters.Add( new SqlParameter( "Handle", handle.Username ) );
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

        public IList<TwitterHandle> GetAllRecords()
        {
            IList<TwitterHandle> handles = new List<TwitterHandle>();

            if ( _connection.State != ConnectionState.Open )
            {
                ConnectToDatabase();
            }

            using ( _connection )
            {
                var command = new SqlCommand( "SELECT * FROM ScoredHandles", _connection );

                var reader = command.ExecuteReader();

                while ( reader.Read() )
                {
                    // write the data on to the screen
                    Console.WriteLine( String.Format( "{0} \t | {1} \t | {2} \t | {3}",
                        // call the objects from their index
                        reader[0], reader[1], reader[2], reader[3] ) );

                }
            }

            return handles;
        }

       
    }
}
