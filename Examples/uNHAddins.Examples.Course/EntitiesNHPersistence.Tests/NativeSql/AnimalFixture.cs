using NHibernate;
using NUnit.Framework;

namespace uNHAddins.Examples.Course.Tests.NativeSql
{
	using System;
	using System.Collections.Generic;
	using YourPrjDomain.NativeSql;

	[TestFixture]
	public class AnimalFixture : TestCase
	{
		protected override System.Collections.IList Mappings
		{
			get { return new string[] { "NativeSql.Animal.hbm.xml" }; }
		}

		protected override void OnSetUp()
		{
			List<Type> testsForDialects = new List<Type>();
			testsForDialects.Add(typeof(NHibernate.Dialect.MsSql2000Dialect));
			testsForDialects.Add(typeof(NHibernate.Dialect.MsSql2005Dialect));

			if (!testsForDialects.Contains(NHibernate.Dialect.Dialect.GetDialect(cfg.Properties).GetType()))
				Assert.Ignore("Not supported by:" + NHibernate.Dialect.Dialect.GetDialect(cfg.Properties).GetType());

			base.OnSetUp();
		}

		// This test shows how CRUD queries can be overriden without changing the code (it's the same as in Core.AnimalFixture.cs)
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
		public void NativeSpecificQuery()
		{
			using (ISession s = OpenSession())
			{
				// Note: It's recommended to include the queries to hbm.xml files
				ISQLQuery query = s.CreateSQLQuery("select @@version as ver");
				query.AddScalar("ver", NHibernateUtil.String);
				object version = query.UniqueResult();

				Assert.IsNotNull(version as string);
				Assert.IsNotEmpty(version as string);

				log.Debug(version);
			}
		}
	}
}