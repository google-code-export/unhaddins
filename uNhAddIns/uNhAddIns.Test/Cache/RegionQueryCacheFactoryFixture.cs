using NHibernate.Cfg;
using NHibernate.Engine;
using NUnit.Framework;
using uNhAddIns.Cache;

namespace uNhAddIns.Test.Cache
{
	[TestFixture]
	public class RegionQueryCacheFactoryFixture
	{
		[Test]
		public void ShouldCreateCustomRegionQueryCacheResolver()
		{
			const string regionName = "RefineSearchStatistics";
			const string regionName1 = "Statistics";
			var nhcfg = new Configuration().Configure();
			nhcfg.SetProperty(Environment.CacheProvider, typeof(NHibernate.Cache.HashtableCacheProvider).AssemblyQualifiedName);
			nhcfg.QueryCache().ResolveRegion(regionName).Using<CustomQcStub>().TolerantWith("ATable");
			nhcfg.QueryCache().ResolveRegion(regionName1).Using<TolerantQueryCache>().TolerantWith("ATable", "ATable2", "ATable1");

			var sfi = (ISessionFactoryImplementor) nhcfg.BuildSessionFactory();
			
			sfi.GetQueryCache(regionName).Should().Be.OfType<CustomQcStub>();

			sfi.GetQueryCache(regionName1)
				.Should().Be.OfType<TolerantQueryCache>()
				.And.ValueOf.ToleratedSpaces
					.Should().Have.SameValuesAs(new[] {"ATable", "ATable2", "ATable1"});
			sfi.Dispose();
		}
	}
}