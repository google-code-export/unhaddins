using System;
using NHibernate;
using NHibernate.Impl;
using NUnit.Framework;
using uNhAddIns.Pagination;

namespace uNhAddIns.Test.Pagination
{
	[TestFixture]
	public class AbstractPaginableQueryFixture
	{
		public class WrongImplementation<T>: AbstractPaginableQuery<T>
		{
			private readonly IDetachedQuery dq;

			public WrongImplementation(IDetachedQuery dq)
			{
				this.dq = dq;
			}

			#region Overrides of AbstractPaginableQuery<T>

			protected override IDetachedQuery DetachedQuery
			{
				get { return dq; }
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
			Assert.Throws<ArgumentNullException>(()=> wi.ListAll());
			Assert.Throws<ArgumentNullException>(() => wi.GetPage(3,1));
		}

		[Test]
		public void SelfProtectionForSession()
		{
			var wi = new WrongImplementation<Foo>(new DetachedQuery("from Foo"));
			Assert.Throws<ArgumentException>(() => wi.ListAll());
		}
	}
}