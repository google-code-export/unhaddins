using System.Collections;
using NHibernate;
using NUnit.Framework;
using YourPrjDomain.OptimisticLock;

namespace uNHAddins.Examples.Course.Tests.OptimisticLock
{
	[TestFixture]
	public class OLFixture : TestCase
	{
		protected override IList Mappings
		{
			get { return new string[] { "OptimisticLock.OL.hbm.xml" }; }
		}

		[Test]
		public void Cuncurrency()
		{
			object savedId;
			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				Person p = new Person("Fabio");
				s.Save(p);
				t.Commit();
				savedId = p.Id;
			}

			ISession s1 = OpenSession();
			ITransaction t1 = s1.BeginTransaction();
			ISession s2 = OpenSession();
			ITransaction t2 = s2.BeginTransaction();
			Person p1 = s1.Get<Person>(savedId);
			Person p2 = s2.Get<Person>(savedId);
			p1.Name = "Dario";
			s1.Update(p1);
			t1.Commit();
			s1.Close();
			try
			{
				p2.Name = "Diego";
				s2.Update(p2);
				t2.Commit();
			}
			catch (StaleObjectStateException)
			{
				// All ok
			}
			finally
			{
				s2.Close();
			}

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				s.Delete("from Person");
				t.Commit();
			}
		}
	}
}
