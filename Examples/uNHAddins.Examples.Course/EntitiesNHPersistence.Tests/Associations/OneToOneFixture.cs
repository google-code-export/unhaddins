using System.Collections;
using NHibernate;
using NUnit.Framework;
using YourPrjDomain.Associations.OneToOne;

namespace uNHAddins.Examples.Course.Tests.Associations
{
	[TestFixture]
	public class OneToOneFixture : TestCase
	{
		protected override IList Mappings
		{
			get { return new string[] { "Associations.OneToOne.Mappings.hbm.xml" }; }
		}

		protected override void OnTearDown()
		{
			using (ISession s = OpenSession())
			{
				s.Delete("from Manager");
				s.Delete("from Department");
				s.Flush();
			}
		}

		[Test]
		public void SaveOneToOne()
		{
			using (ISession s = OpenSession())
			using (ITransaction trx = s.BeginTransaction())
			{
				Department d = new Department();
				d.Name = "Marketing";

				Manager m = new Manager();
				m.FullName = "Roberto Carlos Bermudez";

				d.ManagedBy = m;
				m.ManageTo = d;

				s.Save(d);
				s.Save(m);
				trx.Commit();
			}

			using (ISession s = OpenSession())
			{
				log.Debug("Get the Manager");
				Manager m = s.Get<Manager>(1);
				log.DebugFormat("Name of the Manager: {0}", m.FullName);
			}
		}
	}
}