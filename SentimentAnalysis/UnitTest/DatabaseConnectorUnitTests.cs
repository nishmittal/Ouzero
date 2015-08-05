using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Linq;
using SentimentAnalysis.Csv;
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
        public void DoCsvExport()
        {
            var session = FluentNHibernateHelper.OpenSession();
            var records = session.Query<ScoredHandle>().ToList();
            var path = @"C:\Users\Nishant\Desktop\Dropbox\Ouzero\DatabaseExport.csv";

            using(var writer = new CsvFileWriter(path))
                foreach(var h in records)
                {
                    var row = new CsvRow
                    {
                        h.Username,
                        h.ImgUrl,
                        h.Followers.ToString(),
                        h.Friends.ToString(),
                        ((int) h.RetweetRate).ToString(),
                        ((int) h.FavouriteRate).ToString(),
                        h.Bio,
                        h.Website,
                        h.AlexaRank.ToString(),
                        h.AlexaBounce.ToString(),
                        h.AlexaPagePerf.ToString(),
                        h.AlexaTraffic.ToString(),
                        h.Category,
                        ((int) h.Score).ToString()
                        //h.Location
                        //h.Name
                    };

                    writer.WriteRow(row);
                }
            
        }

        [TestMethod]
        public void CheckNumberOfRecords()
        {
            Console.WriteLine(DatabaseConnector.GetNumberOfRows());
        }

    }
}
