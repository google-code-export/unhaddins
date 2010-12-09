using System.Collections;
using NHibernate;
using NUnit.Framework;
using YourPrjDomain.LazyLoad;
using System.Collections.Generic;

namespace uNHAddins.Examples.Course.Tests.LazyLoad
{
	[TestFixture]
	public class BehaviourFixture : TestCase
	{
		protected override IList Mappings
		{
			get { return new string[] { "LazyLoad.PersonAndAccount.hbm.xml" }; }
		}

		protected override void OnTearDown()
		{
			using (ISession s = OpenSession())
			{
				s.Delete("from Account");
				s.Delete("from Person");
				s.Flush();
			}
		}

		[Test]
		public void HitDbWhenNeeded()
		{
			using (new TemporaryOffLog("NHibernate.SQL"))
			using (ISession s = OpenSession())
			using (ITransaction transaction = s.BeginTransaction())
			{
				Person p = new Person();
				p.Name = "John";
				for (int i = 0; i < 10; i++)
				{
					Account a = new Account();
					a.BelongTo = p;
					p.Accounts.Add(a);
				}

				s.Save(p);
				transaction.Commit();
			}

			using (ISession s = OpenSession())
			using (ITransaction transaction = s.BeginTransaction())
			{
				log.Debug("Load 2 accounts but no hit to DB");
				Account a1 = s.Load<Account>(1);
				Account a2 = s.Load<Account>(2);

				log.Debug("See the person, and hit to DB");
				Person p1 = a1.BelongTo;
				Assert.IsNotNull(p1);
				Assert.AreEqual("John", p1.Name);

				log.Debug("Iterate on Accounts");
				foreach (Account item in p1.Accounts)
				{
					log.DebugFormat("item: {0}", item.GetType().ToString());
				}
			}
		}

		[Test]
		public void GetInsteadLoad()
		{
			int personSaved;
			using (new TemporaryOffLog("NHibernate.SQL"))
			using (ISession s = OpenSession())
			using (ITransaction transaction = s.BeginTransaction())
			{
				Person p = new Person();
				p.Name = "John";
				for (int i = 0; i < 10; i++)
				{
					Account a = new Account();
					a.BelongTo = p;
					p.Accounts.Add(a);
				}

				s.Save(p);
				personSaved = p.Id;
				transaction.Commit();
			} 
			
			using (ISession s = OpenSession())
			using (ITransaction transaction = s.BeginTransaction())
			{
				Person p = s.Get<Person>(personSaved);
				log.Debug("---- Person Get ----");

				log.Debug("A collection is fetched when the application invokes an operation upon that collection. (ExtraLazy not supported jet)");
				Account a = p.Accounts[0];
			}
		}

		[Test]
		public void BatchFeching()
		{
			// Run the test with and without batch-size="10"

			using (new TemporaryOffLog("NHibernate.SQL"))
			using (ISession s = OpenSession())
			using (ITransaction transaction = s.BeginTransaction())
			{
				for (int j = 0; j < 10; j++)
				{
					Person p = new Person();
					p.Name = "P"+j;
					for (int i = 0; i < 10; i++)
					{
						Account a = new Account();
						a.BelongTo = p;
						p.Accounts.Add(a);
					}

					s.Save(p);
				}
				transaction.Commit();
			}

			using (ISession s = OpenSession())
			using (ITransaction transaction = s.BeginTransaction())
			{
				log.Debug("Load all Person");
				IList<Person> lp = s.CreateQuery("from Person").List<Person>();
				log.DebugFormat("Load all account collection for the first 10 persons.");

				foreach (Person person in lp)
				{
					Account a = person.Accounts[0];
				}
			}
		}
	}
}
