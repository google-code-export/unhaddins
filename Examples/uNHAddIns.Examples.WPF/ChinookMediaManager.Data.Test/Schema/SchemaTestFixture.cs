using System.Collections.Generic;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Metadata;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace ChinookMediaManager.Data.Test.Schema
{
	[TestFixture]
	public class SchemaTestFixture
	{
		#region Setup/Teardown

		[SetUp]
		public void SetUp()
		{
			configuration = new Configuration();
			configuration.Configure("hibernate-readonly.cfg.xml");
		}

		#endregion

		private Configuration configuration;

		[Test]
		public void AllNHibernateMappingAreOkay()
		{
			ISessionFactory sessionFactory = configuration.BuildSessionFactory();


			using (ISession session = sessionFactory.OpenSession())
			{
				IDictionary<string, IClassMetadata> allClassMetadata = session.SessionFactory.GetAllClassMetadata();

				foreach (var entry in allClassMetadata)
				{
					session.CreateCriteria(entry.Key)
						.SetMaxResults(0).List();
				}
			}
		}

		[Test, Ignore, Explicit]
		public void CrearSchema()
		{
			var schemaCreator = new SchemaExport(configuration);
			schemaCreator.Create(false, true);
		}

		[Test, Ignore, Explicit]
		public void ValidateSchema()
		{
			var schemaValidator = new SchemaValidator(configuration);
			schemaValidator.Validate();
		}
	}
}