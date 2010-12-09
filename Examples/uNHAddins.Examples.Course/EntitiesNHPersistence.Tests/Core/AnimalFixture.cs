using log4net.Core;
using NHibernate;
using NUnit.Framework;
using YourPrjDomain.Core;

namespace uNHAddins.Examples.Course.Tests.Core
{
	[TestFixture]
	public class AnimalFixture : TestCase
	{
		protected override System.Collections.IList Mappings
		{
			get { return new string[] { "Core.Animal.hbm.xml" }; }
		}

		[Test]
		public void CRUD()
		{
			Animal a = new Animal("Dog", 8.46f);

			// Make entity persistent
			log.Debug("Make entity persistent");
			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				s.Save(a);
				t.Commit();
			}
			object savedId = a.Id;

			// Retrive (check if the entity was well persisted)
			log.Debug("Retrive (check if the entity was well persisted)");
			using (ISession s = OpenSession())
			{
				a = s.Get<Animal>(savedId);
			}
			Assert.IsNotNull(a);
			Assert.AreEqual("Dog", a.Description);
			Assert.AreEqual(8.46f, a.BodyWeight);

			// Update
			log.Debug("Update");
			a.Description = "Lamb";
			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				s.Update(a);
				t.Commit();
			}
			// Retrive (eventually check that what is changed is exactly what we expected) 
			log.Debug("Retrive (eventually check that what is changed is exactly what we expected)");
			using (ISession s = OpenSession())
			{
				a = s.Get<Animal>(savedId);
			}
			Assert.AreEqual("Lamb", a.Description);
			Assert.AreEqual(8.46f, a.BodyWeight);

			// Delete 
			log.Debug("Delete");
			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				s.Delete(a);
				t.Commit();
			}

			// Retrive (in general this is not necessary since TestCase check for DB cleaned at the and of each test)
			log.Debug("Retrive (final)");
			using (ISession s = OpenSession())
			{
				a = s.Get<Animal>(savedId);
			}
			Assert.IsNull(a);
		}

		[Test]
		public void CRUDInOneSession()
		{
			Animal a = new Animal("Dog", 8.46f);

			using (ISession s = OpenSession())
			//using (ITransaction t = s.BeginTransaction())
			{
				s.Save(a);
				a.Description = "Lamb";
				s.Update(a);
				s.Delete(a);
			//	t.Commit();
			}
		}

		[Test]
		public void IntercepSQL()
		{
			log.Debug("Retrive (in general this is not necessary since TestCase check for DB cleaned at the and of each test)");
			using (ISession s = OpenSession())
			{
				using (SqlLogSpy spy = new SqlLogSpy())
				{
					s.CreateQuery("from Animal").List<Animal>();
					LoggingEvent[] le = spy.Appender.GetEvents();
					Assert.AreEqual(1, le.Length);
					log.Debug("The SQL executed was:" + le[0].MessageObject);
				}
			}
		}
	}
}