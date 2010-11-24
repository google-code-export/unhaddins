using System.Collections;
using NHibernate;
using NHibernate.Expressions;
using NUnit.Framework;
using YourPrjDomain.Core;

namespace uNHAddins.Examples.Course.Tests.Core
{
	[TestFixture]
	public class CriteriaAndFirstLevelCacheFixture : TestCase
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

				log.Debug("Query the Suricato for First time");
				Suricato suricato1 = s.CreateCriteria(typeof(Suricato))
															.Add(Expression.Eq("Id", 1))
															.UniqueResult() as Suricato;

				log.Debug("Get/Load the Suricato for Second time, NH doesn't go to the DB");
				//Suricato suricato2 = s.Load<Suricato>(1);
				Suricato suricato2 = s.Get<Suricato>(1);
				Assert.IsTrue(suricato1 == suricato2);

				log.Debug("Done");
			}
		}
	}
}