using System.Data;
using AutoQueryViewer.ServiceModel;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace AutoQueryViewer.Tests
{
    [Ignore("One-off Admin Tasks")]
    public class AdminTasks
    {
        private IAppSettings config;
        IAppSettings Config
        {
            get { return config ?? (config = new TextFileSettings("~/appsettings.txt".MapProjectPath())); }
        }

        private IDbConnectionFactory factory;
        private IDbConnectionFactory Factory
        {
            get
            {
                return factory ?? (factory = new OrmLiteConnectionFactory(
                    Config.GetString("ConnectionString"), PostgreSqlDialect.Provider));
            }
        }

        public IDbConnection OpenDbConnection()
        {
            return Factory.OpenDbConnection();
        }

        [Test]
        public void Add_ServiceStack_AutoQueryServices()
        {
            using (var db = OpenDbConnection())
            {
                db.DropAndCreateTable<AutoQueryService>();
            }
        }
    }
}