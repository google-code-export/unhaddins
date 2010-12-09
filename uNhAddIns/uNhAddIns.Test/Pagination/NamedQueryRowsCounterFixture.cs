using System;
using NHibernate.Impl;
using NUnit.Framework;
using uNhAddIns.Pagination;
using uNhAddIns.TestUtils.NhIntegration;

namespace uNhAddIns.Test.Pagination
{
	[TestFixture]
	public class NamedQueryRowsCounterFixture : PaginationTestBase
	{
		[Test]
		public void CtorProtection()
		{
			string nothing = null;
			DetachedNamedQuery dq = null;
			Assert.Throws<ArgumentNullException>(() => new NamedQueryRowsCounter(nothing));
			Assert.Throws<ArgumentNullException>(() => new NamedQueryRowsCounter(dq));
		}

		[Test]
		public void RowsCount()
		{
			IRowsCounter rc = new NamedQueryRowsCounter("Foo.Count.All");
			SessionFactory.EncloseInTransaction(s => Assert.That(rc.GetRowsCount(s), Is.EqualTo(TotalFoo)));
		}

		[Test]
		public void RowsCountUsingParameters()
		{
			var q = new DetachedNamedQuery("Foo.Count.Parameters");
			q.SetString("p1", "%1_");
			IRowsCounter rc = new NamedQueryRowsCounter(q);
			SessionFactory.EncloseInTransaction(s => Assert.That(rc.GetRowsCount(s), Is.EqualTo(5)));
		}

		[Test]
		public void UsingParametersTemplate()
		{
			var q = new DetachedNamedQuery("Foo.Parameters");
			IRowsCounter rc = new NamedQueryRowsCounter("Foo.Count.Parameters", q);
			q.SetString("p1", "%1_");
			SessionFactory.EncloseInTransaction(s => Assert.That(rc.GetRowsCount(s), Is.EqualTo(5)));
		}
	}
}