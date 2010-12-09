using System.Collections;
using NHibernate;
using NUnit.Framework;
using YourPrjDomain.Core;

namespace uNHAddins.Examples.Course.Tests.Core
{
	[TestFixture]
	public class NamedQueryAndFirstLevelCacheFixture : TestCase
	{
		protected override IList Mappings
		{
			get { return new string[] { "Core.Animal.hbm.xml" }; }
		}


		protected override void OnTearDown()
		{
			using (ISession s = OpenSession())
			{
				s.Delete("from Suricato");
				s.Flush();
			}
		}

		[Test]
		public void CreateSuricatoAndQueryIt()
		{
			Suricato suri = new Suricato();
			suri.BodyWeight = 15;
			suri.Leader = true;
			suri.Description = "Cool Animal";

			using (new TemporaryOffLog("NHibernate.SQL"))
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					s.Save(suri);

					tx.Commit();
				}
			}

			using (ISession s = OpenSession())
			{
				log.Debug("Query the Suricato for First time");
				Suricato suricato1 = s.GetNamedQuery("SuricatoById").SetInt32("value", 1).UniqueResult() as Suricato;

				Assert.IsNotNull(suricato1);

				log.Debug("Get/Load the Suricato for Second time");
				//Suricato suricato2 = s.Load<Suricato>(1);
				Suricato suricato2 = s.Get<Suricato>(1);
				Assert.IsNotNull(suricato2);
			}
		}
	}
}