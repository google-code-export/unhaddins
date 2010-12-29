using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate;
using NUnit.Framework;
using YourPrjDomain.Criteria;

namespace uNHAddins.Examples.Course.Tests.Criteria
{
	[TestFixture]
	public class CriteriaFixture : TestCase
	{
		protected override IList Mappings
		{
			get { return new string[] {"Criteria.Mappings.hbm.xml"}; }
		}

		protected override void OnTearDown()
		{
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				s.Delete("from Customer");
				s.Flush();
				tx.Commit();
			}
		}

		#region Loagind data
		protected override void OnSetUp()
		{
			using (new TemporaryOffLog("NHibernate.SQL"))
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				//--------- Customer 1

				Customer c = new Customer();
				c.FirstName = "Jose";
				c.LastName = "Flores";
				c.CustomerId = "0001-1234";
				c.City = new City("Leandro N. Alem", "3315");
				c.Born = DateTime.Parse("12/12/1972");
				c.AddAccount(new Account(1, MoneyType.Euro, true, DateTime.Parse("1/1/1981"), 1500d));
				c.AddAccount(new Account(2, MoneyType.Dollar, true, DateTime.Today, 2000d));
				s.Save(c);

				//--------- Customer 2

				Customer c2 = new Customer();
				c2.FirstName = "Jorge";
				c2.LastName = "Martinez";
				c2.CustomerId = "0001-2343";
				c2.City = new City("Posadas", "3434");
				c2.Born = DateTime.Parse("1/1/1956");
				c2.AddAccount(new Account(1, MoneyType.Euro, true, DateTime.Parse("1/1/1982"), 3500d));
				c2.AddAccount(new Account(2, MoneyType.Dollar, false, DateTime.Today, 3000d));
				c2.AddAccount(new Account(3, MoneyType.Dollar, true, DateTime.Today, 10000d));
				s.Save(c2);


				//--------- Customer 3

				Customer c3 = new Customer();
				c3.FirstName = "Maria";
				c3.LastName = "Flores";
				c3.CustomerId = "0001-3345";
				c3.City = new City("Posadas", "3434");
				c3.Born = DateTime.Parse("1/1/1960");
				c3.AddAccount(new Account(1, MoneyType.Dollar, true, DateTime.Parse("1/1/1983"), 10000d));
				s.Save(c3);

				s.Flush();

				tx.Commit();
			}
		}
		#endregion

		[Test]
		public void FirstName()
		{
			using(ISession s =  OpenSession())
			{
				CustomerFinder finder = new CustomerFinder();

				finder.FirstName = "Jo";
				finder.AccountMoney = MoneyType.Euro;

				IList<Customer> list = finder.Find(s);

				Assert.AreEqual(2,list.Count);
			}
		}

		[Test]
		public void QueryUsingAccountInfo()
		{
			using (ISession s = OpenSession())
			{
				CustomerFinder finder = new CustomerFinder();
				finder.AccountActive = true;
				finder.AccountSizeMin = 10000d;
				
				log.Debug("--- Run the query with IsActive and Size are taken into account ---");
				IList<Customer> list = finder.Find(s);
				Assert.AreEqual(2, list.Count);

				log.Debug("--- Run the query with IsActive, Size and Opened are taken into account ---");
				finder.AccountOpenedStart = DateTime.Parse("1/1/1983");
				finder.AccountOpenedEnd = DateTime.Parse("1/1/1984");

				IList<Customer> list2 = finder.Find(s);
				Assert.AreEqual(1, list2.Count);
			}
		}

		[Test]
		public void QueryAndTheResultHasNotDistinct()
		{
			using (ISession s = OpenSession())
			{
				CustomerFinder finder = new CustomerFinder();
				finder.AccountMoney = MoneyType.Dollar;

				IList<Customer> list = finder.Find(s);
				Assert.AreEqual(4, list.Count);
			}
		}
	}
}