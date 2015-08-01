using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using SentimentAnalysis.Properties;

namespace SentimentAnalysis.Database
{
    public static class FluentNHibernateHelper
    {
        private static ISessionFactory _sessionFactory;

        private static ISessionFactory CreateSessionFactory()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", GetProjectDirectory());
            _sessionFactory =
                Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2012.ConnectionString(Settings.Default.DbConnectionString))
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Program>())
                    .BuildSessionFactory();

            return _sessionFactory;
        }

        public static ISession OpenSession()
        {
            return CreateSessionFactory().OpenSession();
        }

        public static IStatelessSession OpenStatelessSession()
        {
            return CreateSessionFactory().OpenStatelessSession();
        }

        private static string GetProjectDirectory()
        {
            var path = Environment.CurrentDirectory;

            path = path.Replace("\\bin\\Debug", "");

            path = path.Replace("\\UnitTest", "");

            path = path + "\\SentimentAnalysis";

            if(path.EndsWith("Ouzero\\SentimentAnalysis\\SentimentAnalysis"))
            {
                return path;
            }
            throw new Exception("Path couldn't be worked out.");
        }
    }
}
