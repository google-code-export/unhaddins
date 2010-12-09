using System.Collections.Generic;
using NHibernate;
using NUnit.Framework;
using uNhAddIns.Pagination;
using uNhAddIns.TestUtils.NhIntegration;

namespace uNhAddIns.Test.Pagination
{
	public abstract class AbstractPaginableCommonTest : PaginationTestBase
	{
		protected abstract IPaginable<Foo> GetAllPaginable(ISession session);
		protected abstract IPaginable<Foo> GetPaginableWithLikeRestriction(ISession session);

		[Test]
		public void ListAll()
		{
			using (ISession session = SessionFactory.OpenSession())
			{
				IPaginable<Foo> pg = GetAllPaginable(session);
				IList<Foo> l = pg.ListAll();
				Assert.AreEqual(15, l.Count);
			}
		}

		[Test]
		public void GetPage()
		{
			// Page count start from 1
			using (ISession session = SessionFactory.OpenSession())
			{
				IPaginable<Foo> pg = GetAllPaginable(session);
				IList<Foo> l = pg.GetPage(3, 2);
				Assert.AreEqual(3, l.Count);
				Assert.AreEqual("N3", l[0].Name);
				Assert.AreEqual("N4", l[1].Name);
				Assert.AreEqual("N5", l[2].Name);

				l = pg.GetPage(2, 1);
				Assert.AreEqual(2, l.Count);
				Assert.AreEqual("N0", l[0].Name);
				Assert.AreEqual("N1", l[1].Name);

				// If pageSize=10 the page 2 have 5 elements
				l = pg.GetPage(10, 2);
				Assert.AreEqual(5, l.Count);
			}

			// Add an element from other session
			object savedId = null;
			SessionFactory.EncloseInTransaction(s => savedId = s.Save(new Foo("NZ", "DZ")));

			// Reload the same page and have the new element
			using (ISession session = SessionFactory.OpenSession())
			{
				IPaginable<Foo> pg = GetAllPaginable(session);
				IList<Foo> l = pg.GetPage(10, 2);
				// If pageSize=10 the page 2 have 6 elements
				Assert.AreEqual(6, l.Count);
			}

			SessionFactory.EncloseInTransaction(s => s.Delete(s.Get<Foo>(savedId)));
		}

		[Test]
		public void PaginatorWorkFine()
		{
			using (ISession session = SessionFactory.OpenSession())
			{
				var ptor = new Paginator<Foo>(3, GetAllPaginable(session));
				IList<Foo> entities = ptor.GetFirstPage();
				Assert.AreEqual(3, entities.Count);
				Assert.AreEqual(1, ptor.CurrentPageNumber);

				entities = ptor.GetNextPage();
				Assert.AreEqual(3, entities.Count);
				Assert.AreEqual(2, ptor.CurrentPageNumber);
				Assert.AreEqual("N3", entities[0].Name);
				Assert.AreEqual("N4", entities[1].Name);
				Assert.AreEqual("N5", entities[2].Name);

				entities = ptor.GetPage(4);
				Assert.AreEqual(3, entities.Count);
				Assert.AreEqual(4, ptor.CurrentPageNumber);
				Assert.AreEqual("N9", entities[0].Name);
				Assert.AreEqual("N10", entities[1].Name);
				Assert.AreEqual("N11", entities[2].Name);

				entities = ptor.GetPreviousPage();
				Assert.AreEqual(3, entities.Count);
				Assert.AreEqual(3, ptor.CurrentPageNumber);
				Assert.AreEqual("N6", entities[0].Name);
				Assert.AreEqual("N7", entities[1].Name);
				Assert.AreEqual("N8", entities[2].Name);
			}
		}
	}
}