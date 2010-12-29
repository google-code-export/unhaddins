using System.Collections;
using System.Collections.Generic;
using NHibernate;
using NUnit.Framework;
using YourPrjDomain.Associations.ManyToMany;

namespace uNHAddins.Examples.Course.Tests.Associations
{
	[TestFixture]
	public class ManyToManyFixture : TestCase
	{
		protected override IList Mappings
		{
			get { return new string[] {"Associations.ManyToMany.Mappings.hbm.xml"}; }
		}

		protected override void OnTearDown()
		{
			using (ISession s = OpenSession())
			{
				s.Delete("from Item");
				s.Delete("from Category");
				s.Flush();
			}
		}

		[Test]
		public void ManyToMany()
		{
			log.Debug("----- Create -----");
			using (ISession s = OpenSession())
			using(ITransaction trx = s.BeginTransaction())
			{
				Item i1 = new Item("item1");
				//Item i2 = new Item("item2");
				Category c1 = new Category("category2");
				s.Save(c1);
				Category c2 = new Category("category1");
				s.Save(c2);
				
				i1.Categories.Add(c1);
				i1.Categories.Add(c2);

				s.Save(i1);
				s.Flush();
				trx.Commit();
			}

			log.Debug("----- Load Items -----");
			using(ISession s = OpenSession())
			{
				IList<Item> l = s.CreateQuery("from Item").List<Item>();
			}

			log.Debug("----- Load Category -----");
			using (ISession s = OpenSession())
			{
				IList<Category> l = s.CreateQuery("from Category").List<Category>();
			}
		}

		[Test]
		public void DeleteAndChange()
		{
			using (new TemporaryOffLog("NHibernate.SQL"))
			using (ISession s = OpenSession())
			using (ITransaction trx = s.BeginTransaction())
			{
				Item i1 = new Item("item1");
				for (int i = 1; i <= 3; i++)
				{
					Category c = new Category("category"+i);
					s.Save(c);
					i1.Categories.Add(c);
				}

				s.Save(i1);
				trx.Commit();
			}

			Item item;
			using (new TemporaryOffLog("NHibernate.SQL"))
			using (ISession s = OpenSession())
			{
				item = s.CreateQuery("from Item").UniqueResult<Item>();
			}

			// Work the Item in detached mode
			Category category = item.Categories[0];
			item.Categories.RemoveAt(0);
			item.Categories[1] = category;

			using (ISession s = OpenSession())
			using (ITransaction trx = s.BeginTransaction())
			{
				s.Update(item);
				trx.Commit();
			}
		}
	}
}