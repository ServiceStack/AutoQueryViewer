using System;
using System.Collections.Generic;
using System.IO;
using AutoQueryViewer.ServiceModel;
using Funq;
using AutoQueryViewer.ServiceInterface;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Caching;
using ServiceStack.Configuration;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Razor;
using ServiceStack.Text;

namespace AutoQueryViewer
{
    public class AppHost : AppHostBase
    {
        /// <summary>
        /// Default constructor.
        /// Base constructor requires a name and assembly to locate web service classes. 
        /// </summary>
        public AppHost()
            : base("AutoQueryViewer", typeof(AutoQueryServices).Assembly)
        {
            var customSettings = new FileInfo(@"~/appsettings.txt".MapHostAbsolutePath());
            AppSettings = customSettings.Exists
                ? (IAppSettings)new TextFileSettings(customSettings.FullName)
                : new AppSettings();
        }

        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
        /// <param name="container"></param>
        public override void Configure(Container container)
        {
            JsConfig.EmitCamelCaseNames = true;

            SetConfig(new HostConfig
            {
                DebugMode = AppSettings.Get("DebugMode", false),
                AddRedirectParamsToQueryString = true,
            });

            this.Plugins.Add(new RazorFormat());

            container.Register<IDbConnectionFactory>(new OrmLiteConnectionFactory(
                AppSettings.GetString("ConnectionString"), PostgreSqlDialect.Provider));

            this.Plugins.Add(new AuthFeature(() => new CustomUserSession(),
                new IAuthProvider[] {
                    new CredentialsAuthProvider(AppSettings),
                }));

            var dbFactory = container.Resolve<IDbConnectionFactory>();
            var authRepo = new OrmLiteAuthRepository(dbFactory);
            container.Register<IUserAuthRepository>(authRepo);
            authRepo.InitSchema();

            container.RegisterAs<OrmLiteCacheClient, ICacheClient>();
            container.Resolve<ICacheClient>().InitSchema();
            container.Register(c => new ContentCache(new MemoryCacheClient()));

            using (var db = dbFactory.OpenDbConnection())
            {
                //db.DropAndCreateTable<AutoQueryService>();
                db.CreateTableIfNotExists<AutoQueryService>();
            }
        }
    }
}