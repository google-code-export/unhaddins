using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Impl;
using NUnit.Framework;
using uNhAddIns.GenericImpl;
using uNhAddIns.Pagination;

namespace uNhAddIns.Test.Pagination
{
	[TestFixture]
	public class PaginableRowsCounterQueryFixture : PaginationTestBase
	{
		[Test]
		public void Ctor()
		{
			using (ISession s = SessionFactory.OpenSession())
			{
				Assert.Throws<ArgumentNullException>(() => new PaginableRowsCounterQuery<Foo>(s, null),
				                                     "Should not accept null query");
			}
			var dq = new DetachedQuery("from Foo f where f.Name like :p1");
			Assert.Throws<ArgumentNullException>(() => new PaginableRowsCounterQuery<Foo>(null, dq),
			                                     "Should not accept null session");
		}

		[Test]
		public void ShouldWorkAsPaginatorAndAsRowCounter()
		{
			var dq = new DetachedQuery("from Foo f where f.Name like :p1");
			dq.SetString("p1", "N_");
			using (ISession s = SessionFactory.OpenSession())
			{
				IPaginable<Foo> fp = new PaginableRowsCounterQuery<Foo>(s, dq);
				IList<Foo> l = fp.GetPage(5, 1);
				Assert.That(l.Count, Is.EqualTo(5));
				Assert.That(((IRowsCounter) fp).GetRowsCount(s), Is.EqualTo(10));
			}
		}

		[Test]
		public void ShouldNotTransformAUnsafeHQL()
		{
			var dq = new DetachedQuery("select f.Name from Foo f");
			using (ISession s = SessionFactory.OpenSession())
			{
				IPaginable<Foo> fp = new PaginableRowsCounterQuery<Foo>(s, dq);
				Assert.Throws<HibernateException>(() => ((IRowsCounter) fp).GetRowsCount(s),
				                                  "Should not transform a query with a select clause.");
			}
		}
	}
}