using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Linq;
using SentimentAnalysis.Database;
using SentimentAnalysis.Entities;

namespace UnitTest
{
    [TestClass]
    public class DatabaseConnectorUnitTests
    {
        [TestMethod]
        public void TestCommit_ShouldHaveOneMoreRecordThanBefore()
        {
            RemoveTestCommitsFromDb();
            var originalNumberOfRows = DatabaseConnector.GetNumberOfRows();
            DoTestCommit("TestCommit");
            var rowsAfterTestCommit = DatabaseConnector.GetNumberOfRows();
            Assert.IsTrue(rowsAfterTestCommit == originalNumberOfRows + 1);
            RemoveTestCommitsFromDb();
        }

        private static void DoTestCommit(string name)
        {
            var session = FluentNHibernateHelper.OpenSession();
            using(session)
            {
                using(var transaction = session.BeginTransaction())
                {
                    var scoredHandle = new ScoredHandle { Username = name, Name = "FluentName", Followers = 10, FavouriteRate = 2, Friends = 10, RetweetRate = 3, ImgUrl = "img", Score = 99, Website = "webby", Category = "Test", Location = "London", AlexaPagePerf = 0, AlexaBounce = 0, AlexaTraffic = 1000, Bio = "Bio", AlexaRank = 0 };
                    session.SaveOrUpdate(scoredHandle);

                    transaction.Commit();
                }
            }
        }

        private static void RemoveTestCommitsFromDb()
        {
            var session = FluentNHibernateHelper.OpenSession();
            using(session)
            {
                var rowsAffected = session.CreateQuery("delete from ScoredHandle where Username = 'TestCommit'").ExecuteUpdate();
                Console.WriteLine(@"Delete, affected rows: " + rowsAffected);
            }
        }

        [TestMethod]
        public void CheckNumberOfRecords()
        {
            Console.WriteLine(DatabaseConnector.GetNumberOfRows());
        }

    }
}
