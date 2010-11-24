using System;
using System.Linq;
using log4net;
using log4net.Config;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace ChinookMediaManager.Data.Test.BaseClasses
{
	/// <summary>
	/// Ported from NH oficial tests.
	/// </summary>
	public abstract class PersistenceFixtureBase
	{
		private const bool OutputDdl = false;
		//protected Configuration cfg;
		protected ISessionFactoryImplementor Sessions;

		private static readonly ILog Log = LogManager.GetLogger(typeof(PersistenceFixtureBase));

		private ISession lastOpenedSession;
		private DebugConnectionProvider connectionProvider;

		

		/// <summary>
		/// Mapping files used in the TestCase
		/// </summary>
		//protected abstract IList<string> Mappings { get; }

		/// <summary>
		/// Assembly to load mapping files from (default is NHibernate.DomainModel).
		/// </summary>
		protected virtual string MappingsAssembly
		{
			get { return "SGF.Data.Impl"; }
		}

		public ISession LastOpenedSession
		{
			get { return lastOpenedSession; }
		}

		static PersistenceFixtureBase()
		{
			XmlConfigurator.Configure();
			DropSchema();
			CreateSchema();
		}

		/// <summary>
		/// Creates the tables used in this TestCase
		/// </summary>
		[TestFixtureSetUp]
		public void TestFixtureSetUp_Initialization()
		{
			try
			{
				//BuildConfiguration();
				//CreateSchema();
				BuildSessionFactory();
			}
			catch (Exception e)
			{
				Log.Error("Error while setting up the test fixture", e);
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
		public void TestFixtureTearDown_Finished()
		{
			Cleanup();
		}

		protected virtual void OnSetUp() { }

		/// <summary>
		/// Set up the test. This method is not overridable, but it calls
		/// <see cref="OnSetUp" /> which is.
		/// </summary>
		[SetUp]
		public void SetUp()
		{
			OnSetUp();
		}

		protected virtual void OnTearDown() { }

		/// <summary>
		/// Checks that the test case cleans up after itself. This method
		/// is not overridable, but it calls <see cref="OnTearDown" /> which is.
		/// </summary>
		[TearDown]
		public void TearDown()
		{
			OnTearDown();

			bool wasClosed = CheckSessionWasClosed();
			bool wasCleaned = CheckDatabaseWasCleaned();
			bool wereConnectionsClosed = CheckConnectionsWereClosed();
			bool fail = !wasClosed || !wasCleaned || !wereConnectionsClosed;

			if (fail)
			{
				Assert.Fail("Test didn't clean up after itself");
			}
		}

		private bool CheckSessionWasClosed()
		{
			if (lastOpenedSession != null && lastOpenedSession.IsOpen)
			{
				Log.Error("Test case didn't close a session, closing");
				lastOpenedSession.Close();
				return false;
			}

			return true;
		}

		private bool CheckDatabaseWasCleaned()
		{
			if (Sessions.GetAllClassMetadata().Count == 0)
			{
				// Return early in the case of no mappings, also avoiding
				// a warning when executing the HQL below.
				return true;
			}

			bool empty;
			using (ISession s = Sessions.OpenSession())
			{
				empty = s.CreateQuery("from System.Object o").List().Count == 0;
			}

			if (!empty)
			{
				Log.Error("Test case didn't clean up the database after itself, re-creating the schema");
				DropSchema();
				CreateSchema();
			}

			return empty;
		}

		private bool CheckConnectionsWereClosed()
		{
			if (connectionProvider == null || !connectionProvider.HasOpenConnections)
			{
				return true;
			}

			Log.Error("Test case didn't close all open connections, closing");
			connectionProvider.CloseAllConnections();
			return false;
		}

		protected static Configuration BuildConfiguration()
		{
			var cfg = new Configuration().Configure("hibernate-cruds.cfg.xml");
			return cfg;
		}

		private static void CreateSchema()
		{
			new SchemaExport(BuildConfiguration()).Create(OutputDdl, true);
		}

		private static void DropSchema()
		{
			new SchemaExport(BuildConfiguration()).Drop(OutputDdl, true);
		}

		protected virtual void BuildSessionFactory()
		{
			Sessions = (ISessionFactoryImplementor)BuildConfiguration().BuildSessionFactory();
			connectionProvider = Sessions.ConnectionProvider as DebugConnectionProvider;
		}

		private void Cleanup()
		{
			Sessions.Close();
			Sessions = null;
			connectionProvider = null;
			lastOpenedSession = null;
		}

		protected virtual ISession OpenSession()
		{
			lastOpenedSession = Sessions.OpenSession();
			return lastOpenedSession;
		}

		#region Properties overridable by subclasses

		//protected virtual void Configure(Configuration configuration)
		//{
		//    //configuration.Configure();
		//}

		#endregion

		protected void CommitInNewSession(Action<ISession> executeInNewSession)
		{
			using (ISession session = Sessions.OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					executeInNewSession(session);
					session.Flush();
					tx.Commit();
				}
			}
		}

		protected bool ExistsInDb<T>(object id)
		{
			using (ISession s = OpenSession())
			{
				return !ReferenceEquals(s.Get<T>(id), null);
			}
		}

		protected void DropAll<T>() where T : class
		{
			CommitInNewSession(s => Array.ForEach(s.CreateCriteria<T>().Future<T>().ToArray(), s.Delete));
		}
		protected void Drop<T>( T obj)
		{
			CommitInNewSession(s=>s.Delete(obj));
		}
		protected void DropById<T>(object id)
		{
			CommitInNewSession(s=> s.Delete(s.Load<T>(id)));
		}
		protected void Save<T>(T obj)
		{
			CommitInNewSession(s => s.Save(obj));
		}

		protected void Save(object obj)
		{
			CommitInNewSession(s => s.Save(obj));		
		}
	}
}