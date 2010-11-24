using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace uNHAddins.Examples.Course.Tests
{
	[TestFixture]
	public class SchemaUtil
	{
		private const bool OutputDdl = false;

		private Configuration cfg;

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			cfg = new Configuration();
			cfg.Configure();
		}

		[Test, Explicit]
		public void CreateSchema()
		{
			new SchemaExport(cfg).Create(OutputDdl, true);
		}

		[Test, Explicit]
		public void DropSchema()
		{
			new SchemaExport(cfg).Drop(OutputDdl, true);
		}
	}
}
