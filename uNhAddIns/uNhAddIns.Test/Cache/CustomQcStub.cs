using System.Collections.Generic;
using NHibernate.Cache;
using NHibernate.Cfg;

namespace uNhAddIns.Test.Cache
{
	public class CustomQcStub : StandardQueryCache
	{
		public CustomQcStub(Settings settings, IDictionary<string, string> props, UpdateTimestampsCache updateTimestampsCache,
		                    string regionName) : base(settings, props, updateTimestampsCache, regionName) {}
	}
}