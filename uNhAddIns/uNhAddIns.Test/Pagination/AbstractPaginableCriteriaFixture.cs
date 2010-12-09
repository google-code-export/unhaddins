using System;
using NHibernate;
using NHibernate.Criterion;
using NUnit.Framework;
using uNhAddIns.Pagination;

namespace uNhAddIns.Test.Pagination
{
	[TestFixture]
	public class AbstractPaginableCriteriaFixture
	{
		public class WrongImplementation<T> : AbstractPaginableCriteria<T>
		{
			private readonly DetachedCriteria dc;
			public WrongImplementation(DetachedCriteria dc)
			{
				this.dc = dc;
			}

			#region Overrides of AbstractPaginableCriteria<T>

			protected override DetachedCriteria Criteria
			{
				get { return dc; }
			}

			public override ISession GetSession()
			{
				return null;
			}

			#endregion
		}

		[Test]
		public void SelfProtectionForQuery()
		{
			var wi = new WrongImplementation<Foo>(null);
			Assert.Throws<ArgumentNullException>(() => wi.ListAll());
			Assert.Throws<ArgumentNullException>(() => wi.GetPage(3, 1));
		}

		[Test]
		public void SelfProtectionForSession()
		{
			var wi = new WrongImplementation<Foo>(DetachedCriteria.For<Foo>());
			Assert.Throws<ArgumentException>(() => wi.ListAll());
		}
	}
}