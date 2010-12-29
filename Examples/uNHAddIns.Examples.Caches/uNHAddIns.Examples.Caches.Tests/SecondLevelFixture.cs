using System.Collections;
using NHibernate;
using NUnit.Framework;
using uNHAddins.Examples.Caches.Tests;
using TestCase=uNHAddins.Examples.Caches.Tests.TestCase;

namespace uNHAddIns.Examples.Caches.Tests
{
	[TestFixture]
	public class SecondLevelFixture : TestCase
	{
		protected override IList Mappings
		{
			get { return new string[] {"Country.hbm.xml"}; }
		}

		protected override void OnTearDown()
		{
			using (ISession s = base.OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				s.Delete("from System.Object");
				t.Commit();
			}
		}

		[Test]
		public void GetFromMultipleSessions()
		{
			using (new SqlLogSpy())
			{
				Country country;

				using (ISession s = base.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					country = new Country("Italy", "Europe");
					s.Save(country);
					t.Commit();
				}

				log.Debug("Reading 100 times:");

				for (int i = 0; i < 100; i++)
				{
					using (ISession s = base.OpenSession())
					{
						s.Get<Country>(country.Id);
					}
				}
			}
		}

		[Test]
		public void QueryCache()
		{
			using (new SqlLogSpy())
			{
				using (ISession s = base.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					s.Save(new Country("Italy", "Europe"));
					s.Save(new Country("Argentina", "America"));
					s.Save(new Country("Peru", "America"));
					s.Save(new Country("Switzerland", "Europe"));
					s.Save(new Country("Japan", "Asia"));
					s.Save(new Country("Chine", "Asia"));
					t.Commit();
				}

				log.Debug("Reading 100 times:");

				for (int i = 0; i < 100; i++)
				{
					using (ISession s = base.OpenSession())
					{
						IQuery query = s.GetNamedQuery("GetCountryByContinent");
						query.SetString("ContinentName", "America");
						query.List();
					}
				}
			}
		}
	}
}