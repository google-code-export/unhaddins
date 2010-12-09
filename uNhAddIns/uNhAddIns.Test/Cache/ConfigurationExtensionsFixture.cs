using NHibernate.Cache;
using NHibernate.Cfg;
using NUnit.Framework;
using uNhAddIns.Cache;

namespace uNhAddIns.Test.Cache
{
	[TestFixture]
	public class ConfigurationExtensionsFixture
	{
		[Test]
		public void FluentConfigurationUsage()
		{
			const string regionName = "RefineSearchStatistics";
			const string regionName1 = "Statistics";
			var nhcfg = new Configuration();

			nhcfg.QueryCache()
				.ResolveRegion(regionName).Using<CustomQcStub>().TolerantWith("ATable");

			nhcfg.GetQueryCacheRegionResolver(regionName)
				.Should().Be.EqualTo(typeof(CustomQcStub));

			nhcfg.GetQueryCacheRegionTolerance(regionName)
				.Should().Have.SameSequenceAs(new[] { "ATable" });

			nhcfg.QueryCache()
				.ResolveRegion(regionName1).Using<StandardQueryCache>().TolerantWith("ATable", "ATable2", "ATable1");

			nhcfg.GetQueryCacheRegionResolver(regionName1)
				.Should().Be.EqualTo(typeof(StandardQueryCache));

			nhcfg.GetQueryCacheRegionTolerance(regionName1)
				.Should().Have.SameSequenceAs(new[] { "ATable", "ATable2", "ATable1" });

			nhcfg.GetProperty(Environment.QueryCacheFactory)
				.Should().Be.EqualTo(typeof (RegionQueryCacheFactory).AssemblyQualifiedName);

			nhcfg.GetProperty(Environment.UseQueryCache).ToLowerInvariant()
				.Should().Be.EqualTo("true");
		}
	}
}