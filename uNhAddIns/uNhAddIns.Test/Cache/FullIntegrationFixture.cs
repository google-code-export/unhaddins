using System;
using System.Threading;
using NHibernate.Caches.SysCache;
using NHibernate.Impl;
using NUnit.Framework;
using uNhAddIns.Cache;
using uNhAddIns.TestUtils.NhIntegration;
using Environment=NHibernate.Cfg.Environment;

namespace uNhAddIns.Test.Cache
{
	[TestFixture, 
	Ignore("Please update the reference NHibernate.Caches.SysCache to a new version pointing to NH3")]
	public class FullIntegrationFixture : FunctionalTestCase
	{
		public class FullIntegrationFixtureSettings : DefaultFunctionalTestSettings
		{
			public FullIntegrationFixtureSettings(IMappingsLoader mappingLoader) : base(mappingLoader) { }
			public override void Configure(NHibernate.Cfg.Configuration configuration)
			{
				base.Configure(configuration);
				configuration.SetProperty(Environment.GenerateStatistics, "true");
				configuration.SetProperty(Environment.CacheProvider,
					typeof(SysCacheProvider).AssemblyQualifiedName);

				configuration.QueryCache()
					.ResolveRegion("SearchStatistics")
					.Using<TolerantQueryCache>()
					.TolerantWith("MusicCDs");
			}
		}

		public FullIntegrationFixture()
		{
			var ml = new NamespaceMappingsLoader(GetType().Assembly, GetType().Namespace);
			var s = new FullIntegrationFixtureSettings(ml);

			Settings = s;
		}

		[Test, Explicit]
		public void FullCreamIntegration()
		{
			// Fill DB
			SessionFactory.EncloseInTransaction(session =>
			{
				for (int i = 0; i < 10; i++)
				{
					session.Save(new MusicCD { Name = "Music" + (i / 2) });
					session.Save(new Antique { Name = "Antique" + (i / 2) });
				}
			});

			// Queries
			var musicQuery =
				new DetachedQuery("select m.Name, count(*) from MusicCD m group by m.Name")
				.SetCacheable(true)
				.SetCacheRegion("SearchStatistics");

			var antiquesQuery =
				new DetachedQuery("select a.Name, count(*) from Antique a group by a.Name")
				.SetCacheable(true)
				.SetCacheRegion("SearchStatistics");

			// Clear SessionFactory Statistics
			SessionFactory.Statistics.Clear();

			// Put in second-level-cache
			SessionFactory.EncloseInTransaction(session =>
			{
				musicQuery.GetExecutableQuery(session).List();
				antiquesQuery.GetExecutableQuery(session).List();
			});

			// Asserts after execution
			SessionFactory.Statistics.QueryCacheHitCount
				.Should("not hit the query cache").Be.EqualTo(0);

			SessionFactory.Statistics.QueryExecutionCount
				.Should("execute both queries").Be.EqualTo(2);

			// Update both tables
			SessionFactory.EncloseInTransaction(session =>
			{
				session.Save(new MusicCD { Name = "New Music" });
				session.Save(new Antique { Name = "New Antique" });
			});

			// Clear SessionFactory Statistics again
			SessionFactory.Statistics.Clear();

			// Execute both queries again
			SessionFactory.EncloseInTransaction(session =>
			{
				musicQuery.GetExecutableQuery(session).List();
				antiquesQuery.GetExecutableQuery(session).List();
			});

			// Asserts after execution
			SessionFactory.Statistics.QueryCacheHitCount
				.Should("Hit the query cache").Be.EqualTo(1);

			SessionFactory.Statistics.QueryExecutionCount
				.Should("execute only the query for Antiques").Be.EqualTo(1);

			// Clear SessionFactory Statistics again
			SessionFactory.Statistics.Clear();

			// Wait for cache expiration
			Thread.Sleep(new TimeSpan(0, 0, 10));
			// Execute
			SessionFactory.EncloseInTransaction(session => musicQuery.GetExecutableQuery(session).List());
			
			SessionFactory.Statistics.QueryCacheHitCount
				.Should("Not hit the query cache because stale").Be.EqualTo(0);

			SessionFactory.Statistics.QueryExecutionCount
				.Should("execute the query for MusicCD").Be.EqualTo(1);

			// Cleanup
			SessionFactory.EncloseInTransaction(session =>
																						{
																							session.Delete("from MusicCD");
																							session.Delete("from Antique");
																						});
		}
	}
}