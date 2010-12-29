using NHibernate;
using NHibernate.Engine;
using uNhAddIns.TestUtils.NhIntegration;

namespace uNhAddIns.Test.Pagination
{
	public class PaginationTestBase : FunctionalTestCase
	{
		public const int TotalFoo = 15;
		private class PaginationTestSettings : DefaultFunctionalTestSettings
		{

			public PaginationTestSettings(IMappingsLoader mappingLoader)
				: base(mappingLoader)
			{
				AssertAllDataRemoved = false;
			}

			public override void AfterSessionFactoryBuilt(ISessionFactoryImplementor sessionFactory)
			{
				using (ISession s = sessionFactory.OpenSession())
				{
					using (ITransaction tx = s.BeginTransaction())
					{
						for (int i = 0; i < TotalFoo; i++)
						{
							var f = new Foo("N" + i, "D" + i);
							s.Save(f);
						}
						tx.Commit();
					}
				}
			}

			public override void BeforeSchemaShutdown(ISessionFactoryImplementor factory)
			{
				using (ISession s = factory.OpenSession())
				{
					using (ITransaction tx = s.BeginTransaction())
					{
						s.Delete("from Foo");
						tx.Commit();
					}
				}
			}
		}

		public PaginationTestBase()
		{
			var ml = new NamespaceMappingsLoader(GetType().Assembly, GetType().Namespace);
			var s = new PaginationTestSettings(ml);

			Settings = s;
		}
	}
}