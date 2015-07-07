using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Linq;
using SentimentAnalysis;
using SentimentAnalysis.Database;
using SentimentAnalysis.Entities;

namespace UnitTest
{
    [TestClass]
    public class DatabaseConnectorUnitTests
    {
        private IDatabaseConnector _dbConnector;
        private SqlConnection _connection;

       [TestMethod]
        public void ConnectToDatase_ValidSqlConnection_ConnectionStateIsOpen()
        {
            //Arrange
            _connection = new SqlConnection( @"Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\Database\ScoredHandles.mdf;Integrated Security=True;Connect Timeout=30" );
            _dbConnector = new DatabaseConnector( _connection );
            //Act
            _dbConnector.ConnectToDatabase();

            //Assert
            Assert.AreEqual(_connection.State, ConnectionState.Open);
        }

        [TestMethod]
        public void InsertScoredHandle_ValidHandle_NewEntryExistsInDatabase()
        {
            //Arrange
            _connection = new SqlConnection( @"Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\Database\ScoredHandles.mdf;Integrated Security=True;Connect Timeout=30" );
            _dbConnector = new DatabaseConnector( _connection );
            _dbConnector.ConnectToDatabase();
            var handle = new TwitterHandle("TestUsername");

            //Act
            _dbConnector.InsertScoredHandle(handle);
            var twitterHandles = _dbConnector.GetAllRecords();

            //Assert
            Assert.IsTrue(twitterHandles.Contains(handle));
        }

        [TestMethod]
        public void Test()
        {
            int originalNumberOfRows;
            int rowsAfterTestCommit;
            using ( var session = FluentNHibernateHelper.OpenSession() )
            {
                IList<ScoredHandle> handles = session.Query<ScoredHandle>().ToList();

                originalNumberOfRows = handles.Count;
            }
            DoTestCommit("T2");
            using (var session = FluentNHibernateHelper.OpenSession())
            {
                IList<ScoredHandle> handles = session.Query<ScoredHandle>().ToList();

                rowsAfterTestCommit = handles.Count;
            }
            Assert.IsTrue(rowsAfterTestCommit == originalNumberOfRows + 1);
        }

        private static void DoTestCommit(string name)
        {
            var session = FluentNHibernateHelper.OpenSession();
            using ( session )
            {
                using ( var transaction = session.BeginTransaction() )
                {
                    var scoredHandle = new ScoredHandle { Username = name, Name = "FluentName", Followers = 10, FavouriteRate = 2, Friends = 10, RetweetRate = 3, ImgUrl = "img", Score = 99, Website = "webby", Category = "Test", Location = "London", AlexaPagePerf = 0, AlexaBounce = 0, AlexaTraffic = 1000, Bio = "Bio", AlexaRank = 0 };
                    session.SaveOrUpdate( scoredHandle );

                    transaction.Commit();
                }
            }
        }
        


    }
}
