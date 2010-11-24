using NHibernate.Cfg;
using NUnit.Framework;
using uNhAddIns.Mapping;

namespace uNhAddIns.Test.Mapping
{
	[TestFixture]
	public class MappingExtensionsFixture
	{
		[Test]
		public void CreateClonedMapping()
		{
			var conf = new Configuration().Configure();
			conf.AddResource("uNhAddIns.Test.Mapping.Simple.hbm.xml", GetType().Assembly);
			var pc = conf.GetClassMapping("uNhAddIns.Test.Mapping.Simple");
			var cloned = pc.Clone();
			cloned.Should().Not.Be.Null();
			cloned.Should().Not.Be.SameInstanceAs(pc);
		}
	}
}