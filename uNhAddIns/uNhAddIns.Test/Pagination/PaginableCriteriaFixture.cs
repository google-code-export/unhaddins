using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using NUnit.Framework;
using uNhAddIns.GenericImpl;
using uNhAddIns.Pagination;

namespace uNhAddIns.Test.Pagination
{
	[TestFixture]
	public class PaginableCriteriaFixture : AbstractPaginableCommonTest
	{
		[Test]
		public void NullArgumentNotAllowed()
		{
			Assert.Throws<ArgumentNullException>(() => new PaginableCriteria<Foo>(null, DetachedCriteria.For<Foo>()));
			using (ISession s = SessionFactory.OpenSession())
			{
				Assert.Throws<ArgumentNullException>(() => new PaginableCriteria<Foo>(s, null));
			}
		}

		[Test]
		public void PaginableCriteriaImplementsRowsCount()
		{
			DetachedCriteria dc = DetachedCriteria.For<Foo>().Add(Restrictions.Like("Name", "N_"));

			using (ISession session = SessionFactory.OpenSession())
			{
				var fp = new PaginableCriteria<Foo>(session, dc);
				IList<Foo> l = fp.GetPage(5, 1);
				Assert.AreEqual(5, l.Count);
				Assert.AreEqual(10, fp.GetRowsCount(session));
			}
		}

		#region Overrides of PaginableTestBase

		protected override IPaginable<Foo> GetAllPaginable(ISession session)
		{
			DetachedCriteria dc = DetachedCriteria.For<Foo>();
			return new PaginableCriteria<Foo>(session, dc);
		}

		protected override IPaginable<Foo> GetPaginableWithLikeRestriction(ISession session)
		{
			DetachedCriteria dc = DetachedCriteria.For<Foo>().Add(Restrictions.Like("Name", "N_"));
			return new PaginableCriteria<Foo>(session, dc);
		}

		#endregion
	}
}