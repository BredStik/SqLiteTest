using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using SqLiteTest.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace SqLiteTest
{
    public class NHibernateSessionFactoryHelper
    {
        private static NHibernate.Cfg.Configuration _nhConfig;
        private static ISessionFactory _instance;
        private static readonly object LockObject = new object();

        static NHibernateSessionFactoryHelper()
        {
            lock (LockObject)
            {
                if (_instance != null)
                    return;

                //fluently configure from hibernate config
                FluentConfiguration fluentConfig = Fluently.Configure()
                    .Database(SQLiteConfiguration.Standard.ConnectionString("Data Source=|DataDirectory|\\nhibernate.db;Version=3").ShowSql().FormatSql());

                ////add fluent mappings, add conventions
                fluentConfig.Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>());

                fluentConfig.ExposeConfiguration(x => { 
                    _nhConfig = x;
                });

                _instance = fluentConfig.BuildSessionFactory();
            }
        }

        public static void ActionWithSession(Action<ISession> action, bool withTransaction = false)
        {
            ISession session = null;
            ITransaction transaction = null;

            lock (LockObject)
            {
                session = _instance.OpenSession();
                
                if (withTransaction)
                    transaction = session.BeginTransaction();
            }

            using (session)
            {
                try
                {
                    action.Invoke(session);

                    if (transaction != null)
                        transaction.Commit();
                }
                catch (Exception)
                {
                    if (transaction != null)
                        transaction.Rollback();
                    throw;
                }
                finally
                {
                    if (transaction != null)
                        transaction.Dispose();
                }
                
            }
        }

        public static void InitDatabase()
        {
            if (ValidateSchema())
                return;

            ActionWithSession(session =>
            {
                var sb = new StringBuilder();
                //the key point is pass your session.Connection here
                new SchemaUpdate(_nhConfig).Execute(false, true); //true, true, false, session.Connection, new StringWriter(sb));
                session.Flush();

                var newUser = new User { Name = "Mathieu", Password = "myweakpassword" };

                session.Save(newUser);
                session.Flush();
            });
        }

        private static bool ValidateSchema()
        {
            NHibernate.Tool.hbm2ddl.SchemaValidator myvalidator = new NHibernate.Tool.hbm2ddl.SchemaValidator(_nhConfig);
            
            try
            {
                myvalidator.Validate();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                myvalidator = null;
            }
        }
    }
}