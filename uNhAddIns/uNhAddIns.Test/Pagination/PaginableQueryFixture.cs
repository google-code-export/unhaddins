using System;
using NHibernate;
using NHibernate.Impl;
using NUnit.Framework;
using uNhAddIns.GenericImpl;
using uNhAddIns.Pagination;

namespace uNhAddIns.Test.Pagination
{
	[TestFixture]
	public class PaginableQueryFixture : AbstractPaginableCommonTest
	{
		[Test]
		public void NullArgumentNotAllowed()
		{
			Assert.Throws<ArgumentNullException>(() => new PaginableQuery<Foo>(null, new DetachedQuery("from Foo")));
			using (ISession s = SessionFactory.OpenSession())
			{
				Assert.Throws<ArgumentNullException>(() => new PaginableQuery<Foo>(s, null));
			}
		}

		#region Overrides of PaginableTestBase

		protected override IPaginable<Foo> GetAllPaginable(ISession session)
		{
			return new PaginableQuery<Foo>(session, new DetachedQuery("from Foo"));
		}

		protected override IPaginable<Foo> GetPaginableWithLikeRestriction(ISession session)
		{
			var query = new DetachedQuery("f where f.Name like :p1");
			query.SetString("p1", "N_%");
			return new PaginableQuery<Foo>(session, query);
		}

		#endregion
	}
}
