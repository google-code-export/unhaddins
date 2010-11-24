using NUnit.Framework;
using YourPrjDomain.Core;
using NHibernate;

namespace uNHAddins.Examples.Course.Tests.Core
{
	[TestFixture]
	public class FirstLevelCacheFixture : TestCase
	{
		protected override System.Collections.IList Mappings
		{
			get { return new string[] { "Core.Animal.hbm.xml" }; }
		}

		[Test]
		public void CreateSuricato()
		{
			Suricato suri = new Suricato();
			suri.BodyWeight = 15;
			suri.Leader = true;
			suri.Description = "Cool Animal";

			using (ISession s = OpenSession())
			{
				log.Debug("Save the Suricato");
				using (ITransaction tx = s.BeginTransaction())
				{
					s.Save(suri);
					log.Debug("Save done");

					tx.Commit();
					log.Debug("Commit done");
				}

				log.Debug("Get the Suricato for First time");
				Suricato suricato = s.Get<Suricato>(1);
				Assert.IsNotNull(suricato);

				log.Debug("Evit the object from First Level Cache (Session)");
				s.Evict(suricato);

				log.Debug("Get the Suricato from the base");
				Suricato suricato2 = s.Get<Suricato>(1);
				Assert.IsNotNull(suricato2);
				log.Debug("Done");
			}
		}

		protected override void OnTearDown()
		{
			using (ISession s = OpenSession())
			{
				s.Delete("from Suricato");
				s.Flush();
			}
		}
	}
}
