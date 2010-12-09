using System;
using NHibernate.Cache;
using NUnit.Framework;
using uNhAddIns.Cache.ConfigurationImpl;

namespace uNhAddIns.Test.Cache
{
	[TestFixture]
	public class TolerantQueryCacheConfExpressionBuilderFixture
	{
		[Test]
		public void CTor()
		{
			Assert.Throws<ArgumentNullException>(() => new TolerantQueryCacheConfExpressionBuilder(null));
			Assert.Throws<ArgumentNullException>(() => new TolerantQueryCacheConfExpressionBuilder(string.Empty));

			var tqc = new TolerantQueryCacheConfExpressionBuilder("myRegion");
			tqc.RegionName.Should().Be.EqualTo("myRegion");

			tqc.QueryCache.Should().Be.EqualTo(typeof (StandardQueryCache));
			tqc.SpacesTolerance.Should().Be.Empty();
		}

		[Test]
		public void ShouldAcceptCustomQueryCacheImplementation()
		{
			var tqc = new TolerantQueryCacheConfExpressionBuilder("myRegion");
			tqc.SetRegionResolver<CustomQcStub>();
			tqc.QueryCache.Should().Be.EqualTo(typeof(CustomQcStub));
			tqc.SpacesTolerance.Should().Be.Empty();
		}

		[Test]
		public void ShouldSetSpaces()
		{
			var tqc = new TolerantQueryCacheConfExpressionBuilder("myRegion");
			tqc.AddSpace("mySpace");
			tqc.SpacesTolerance.Should().Contain("mySpace");
			tqc.AddSpaces(new[] { "mySpace2", "mySpace1" });
			tqc.SpacesTolerance.Should().Have.SameValuesAs(new[] {"mySpace", "mySpace2", "mySpace1"});
		}

		[Test]
		public void ShouldExcludeEmpty()
		{
			var tqc = new TolerantQueryCacheConfExpressionBuilder("myRegion");
			tqc.AddSpace("");
			tqc.AddSpace(null);
			tqc.SpacesTolerance.Should().Be.Empty();
			tqc.AddSpaces(new[] { "", null, "MySpace" });
			tqc.SpacesTolerance.Should().Have.SameSequenceAs(new[] {"MySpace"});
		}
	}
}