using System.Collections;
using System.Collections.Generic;
using NHibernate;
using NUnit.Framework;
using YourPrjDomain.Filters;

namespace uNHAddins.Examples.Course.Tests.Filters
{
	[TestFixture]
	public class FiltersTest:TestCase
	{
		protected override IList Mappings
		{
			get { return new string[] { "Filters.FilterMapping.hbm.xml" }; }
		}

		[Test]
		public void HqlFilters()
		{
			#region FillDB
			using (new TemporaryOffLog("NHibernate.SQL"))
			using (ISession s = OpenSession())
			using(ITransaction t = s.BeginTransaction())
			{
				for (int i = 0; i < 10; i++)
				{
					Person p = new Person("P" + i);
					p.Live = (i % 2) == 0;
					s.Save(p);
				}
				t.Commit();
			}

			#endregion

			using(ISession session = OpenSession())
			{
				IList<Person> l = GetAll(session);
				log.Debug("Without filters: Load " + l.Count + " persons");
			}

			using (ISession session = OpenSession())
			{
				session.EnableFilter("LiveFilter").SetParameter("isLive", true);
				IList<Person> l = GetAll(session);
				log.Debug("With LiveFilter: Load " + l.Count + " persons");
			}

			using (ISession session = OpenSession())
			{
				session.EnableFilter("LiveFilter").SetParameter("isLive", true);
				session.EnableFilter("MinNameFilter").SetParameter("minName", "P4");
				IList<Person> l = GetAll(session);
				log.Debug("With LiveFilter and MinNameFilter: Load " + l.Count + " persons");
			}

			using (ISession session = OpenSession())
			{
				IList<Person> l = GetAll(session);
				log.Debug("Without filters: Load " + l.Count + " persons");
			}

			#region CleanUp
			using (new TemporaryOffLog("NHibernate.SQL"))
			using(ISession s = OpenSession())
			using(ITransaction t = s.BeginTransaction())
			{
				s.Delete("from Person");
				t.Commit();
			}
			#endregion
		}

		private IList<Person> GetAll(ISession session)
		{
			return session.CreateQuery("from Person").List<Person>();
		}
	}
}
