using System;
using System.Collections;
using System.Data;
using System.Reflection;
using log4net;
using log4net.Config;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace uNHAddins.Examples.Caches.Tests
{
	public abstract class TestCase
	{
		protected Configuration cfg;
		protected ISessionFactory sessions;

		protected static readonly ILog log = LogManager.GetLogger(typeof(TestCase));

		protected NHibernate.Dialect.Dialect Dialect
		{
			get { return NHibernate.Dialect.Dialect.GetDialect(cfg.Properties); }
		}

		protected ISession lastOpenedSession;

		static TestCase()
		{
			// Configure log4net here since configuration through an attribute doesn't always work.
			XmlConfigurator.Configure();
		}

		/// <summary>
		/// Creates the tables used in this TestCase
		/// </summary>
		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			try
			{
				Configure();

				CreateSchema();
				BuildSessionFactory();
			}
			catch (Exception e)
			{
				log.Error("Error while setting up the test fixture", e);
				throw;
			}
		}

		/// <summary>
		/// Removes the tables used in this TestCase.
		/// </summary>
		/// <remarks>
		/// If the tables are not cleaned up sometimes SchemaExport runs into
		/// Sql errors because it can't drop tables because of the FKs.  This 
		/// will occur if the TestCase does not have the same hbm.xml files
		/// included as a previous one.
		/// </remarks>
		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			DropSchema();
			Cleanup();
		}

		protected virtual void OnSetUp()
		{
		}

		/// <summary>
		/// Set up the test. This method is not overridable, but it calls
		/// <see cref="OnSetUp" /> which is.
		/// </summary>
		[SetUp]
		public void SetUp()
		{
			OnSetUp();
		}

		protected virtual void OnTearDown()
		{
		}

		/// <summary>
		/// Checks that the test case cleans up after itself. This method
		/// is not overridable, but it calls <see cref="OnTearDown" /> which is.
		/// </summary>
		[TearDown]
		public void TearDown()
		{
			log.Debug("\n\rTearing Down...");
			InternalTearDown();
		}

		private void InternalTearDown()
		{
			OnTearDown();

			bool wasClosed = CheckSessionWasClosed();
			bool wasCleaned = CheckDatabaseWasCleaned();
			bool fail = !wasClosed || !wasCleaned;

			if (fail)
			{
				Assert.Fail("Test didn't clean up after itself");
			}
		}

		private bool CheckSessionWasClosed()
		{
			if (lastOpenedSession != null && lastOpenedSession.IsOpen)
			{
				log.Error("Test case didn't close a session, closing");
				lastOpenedSession.Close();
				return false;
			}

			return true;
		}

		private bool CheckDatabaseWasCleaned()
		{
			if (sessions.GetAllClassMetadata().Count == 0)
			{
				// Return early in the case of no mappings, also avoiding a warning when executing the HQL below.
				return true;
			}

			bool empty;
			using (ISession s = sessions.OpenSession())
			{
				IList objects = s.CreateQuery("from System.Object o").List();
				empty = objects.Count == 0;
			}

			if (!empty)
			{
				log.Error("Test case didn't clean up the database after itself, re-creating the schema");
				DropSchema();
				CreateSchema();
			}

			return empty;
		}

		private void Configure()
		{
			cfg = new Configuration();

			cfg.Configure();

			Assembly assembly = Assembly.Load(MappingsAssembly);

			if (Mappings != null)
			{
				foreach (string file in Mappings)
					cfg.AddResource(MappingsAssembly + "." + file, assembly);
			}
			else
				cfg.AddAssembly(assembly);

			Configure(cfg);
		}

		protected virtual IList Mappings
		{
			get { return null; }
		}

		protected virtual string MappingsAssembly
		{
			get
			{
				return "uNHAddIns.Examples.Caches";
			}
		}

		private void CreateSchema()
		{
			new SchemaExport(cfg).Create(false, true);
		}

		private void DropSchema()
		{
			new SchemaExport(cfg).Drop(false, true);
		}

		protected virtual void BuildSessionFactory()
		{
			sessions = cfg.BuildSessionFactory();
		}

		private void Cleanup()
		{
			sessions.Close();
			sessions = null;
			lastOpenedSession = null;
			cfg = null;
		}

		public int ExecuteStatement(string sql)
		{
			if (cfg == null)
				cfg = new Configuration();

			using (IConnectionProvider prov = ConnectionProviderFactory.NewConnectionProvider(cfg.Properties))
			{
				IDbConnection conn = prov.GetConnection();

				try
				{
					using (IDbTransaction tran = conn.BeginTransaction())
					using (IDbCommand comm = conn.CreateCommand())
					{
						comm.CommandText = sql;
						comm.Transaction = tran;
						comm.CommandType = CommandType.Text;
						int result = comm.ExecuteNonQuery();
						tran.Commit();
						return result;
					}
				}
				finally
				{
					prov.CloseConnection(conn);
				}
			}
		}

		protected virtual ISession OpenSession()
		{
			lastOpenedSession = sessions.OpenSession();
			return lastOpenedSession;
		}

		protected virtual ISession OpenSession(IInterceptor interceptor)
		{
			lastOpenedSession = sessions.OpenSession(interceptor);
			return lastOpenedSession;
		}

		#region Properties overridable by subclasses

		protected virtual void Configure(Configuration configuration)
		{
			cfg.Configure();
		}

		#endregion
	}
}