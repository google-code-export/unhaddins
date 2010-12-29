using System.Collections.Generic;
using NHibernate;
using NUnit.Framework;

namespace uNhAddIns.Test.UserTypes
{
	[TestFixture]
	public class PropertiesNamesIntegrationFixture: TestCase
	{
		protected override IList<string> Mappings
		{
			get { return new[] { "UserTypes.EntityWithPropNames.hbm.xml" }; }
		}
			object savedId;

		[Test]
		public void SavePropertiesNames()
		{
			FillDb();
			using (ISession s = OpenSession())
			{
				var e = s.Get<EntityWithPropNames>(savedId);
				e.PropertiesNames.Should().Have.SameSequenceAs(new[] { "p1", "p2", "p3" });
			}
			Cleanup();
		}

		private void Cleanup()
		{
			using (ISession s = OpenSession())
			{
				s.Delete(s.Load<EntityWithPropNames>(savedId));
				s.Flush();
			}
		}

		private void FillDb()
		{
			using (ISession s = OpenSession())
			{
				savedId = s.Save(new EntityWithPropNames {PropertiesNames = new HashSet<string> {"p1", "p2", "p3"}});
				s.Flush();
			}
		}

		[Test]
		public void SaveNullPropertiesNames()
		{
			using (ISession s = OpenSession())
			{
				savedId = s.Save(new EntityWithPropNames());
				s.Flush();
			}
 
			using (ISession s = OpenSession())
			{
				var e = s.Get<EntityWithPropNames>(savedId);
				e.PropertiesNames.Should().Be.Null();
			}
			Cleanup();
		}

		[Test]
		public void Query()
		{
			FillDb();
			using (ISession s = OpenSession())
			{
				s.CreateQuery("from EntityWithPropNames e where e.PropertiesNames like '%p2%'")
					.UniqueResult<EntityWithPropNames>()
					.PropertiesNames
					.Should().Have.SameSequenceAs(new[] {"p1", "p2", "p3"});

				s.CreateQuery("select e.PropertiesNames from EntityWithPropNames e").List()[0]
					.Should().Be.OfType<HashSet<string>>()
					.And.ValueOf
					.Should().Have.SameSequenceAs(new[] {"p1", "p2", "p3"});
			}
			Cleanup();
		}
	}
}