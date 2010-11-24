using System;
using NHibernate;
using NUnit.Framework;
using uNhAddIns.DynQuery;
using uNhAddIns.GenericImpl;
using uNhAddIns.Pagination;

namespace uNhAddIns.Test.Pagination
{
	public class PaginableDynQueryFixture : AbstractPaginableCommonTest
	{
		[Test]
		public void NullArgumentNotAllowed()
		{
			Assert.Throws<ArgumentNullException>(() => new PaginableDynQuery<Foo>(null, new DetachedDynQuery(new From("Foo"))));
			using (ISession s = SessionFactory.OpenSession())
			{
				Assert.Throws<ArgumentNullException>(() => new PaginableDynQuery<Foo>(s, null));
			}
		}
		#region Overrides of AbstractPaginableCommonTest

		protected override IPaginable<Foo> GetAllPaginable(ISession session)
		{
			var ddq = new DetachedDynQuery(new From("Foo"));
			return new PaginableDynQuery<Foo>(session, ddq);
		}

		protected override IPaginable<Foo> GetPaginableWithLikeRestriction(ISession session)
		{
			var dynq = new From("Foo f");
			dynq.Where("f.Name like :p1");
			var ddq = new DetachedDynQuery(dynq);
			ddq.SetString("p1", "N_%");
			return new PaginableDynQuery<Foo>(session, ddq);
		}

		#endregion
	}
}