using System;
using System.Collections.Generic;
using log4net;
using log4net.Config;
using log4net.Core;
using log4net.Repository.Hierarchy;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace uNHAddins.Examples.Course.Tests.Ids
{
	// Available IdGenerators

	// Recommended
	//	hilo
	//	sequence
	//	seqhilo
	//	identity
	//	assigned

	// Useful in some specific environment
	//	uuid.hex
	//	uuid.string
	//	guid
	//	guid.comb

	//	foreign (useful with association)

	//	counter (Don't use in a real world application)
	//	increment  (Don't use in a real world application)

	[TestFixture]
	public class IdsFixture
	{

		#region EntityFactory
		private interface IEntityFactory
		{
			object GetNewInstance(int iter);
			string MappingXmlString { get;}
		}

		private class AInt32Factory : IEntityFactory
		{
			private readonly string idStrategy;

			private static readonly string XmlTemplate =
	@"<?xml version='1.0' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
	assembly='uNHAddins.Examples.Course.Tests'
	namespace='uNHAddins.Examples.Course.Tests.Ids'>
	<class name='AInt32'>
		<id name='Id' type='int'>
			<generator class='{0}' />
		</id>
		<property name='Description' type='string' />
	</class>
</hibernate-mapping>";

			public AInt32Factory(string idStrategy)
			{
				this.idStrategy = idStrategy;
			}

			public virtual object GetNewInstance(int iter)
			{
				return new AInt32("Iter=" + iter);
			}

			public string MappingXmlString
			{
				get { return string.Format(XmlTemplate, idStrategy); }
			}
		}

		private class AInt32AssignedFactory : AInt32Factory
		{
			public AInt32AssignedFactory() : base("assigned") { }
			public override object GetNewInstance(int iter)
			{
				AInt32 result = new AInt32("Iter=" + iter);
				result.Id = iter;
				return result;
			}
		}

		private class AInt64Factory : IEntityFactory
		{
			private readonly string idStrategy;

			private static readonly string XmlTemplate =
@"<?xml version='1.0' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
	assembly='uNHAddins.Examples.Course.Tests'
	namespace='uNHAddins.Examples.Course.Tests.Ids'>
	<class name='AInt64'>
		<id name='Id' type='long'>
			<generator class='{0}' />
		</id>
		<property name='Description' type='string' />
	</class>
</hibernate-mapping>";

			public AInt64Factory(string idStrategy)
			{
				this.idStrategy = idStrategy;
			}

			public object GetNewInstance(int iter)
			{
				return new AInt64("Iter=" + iter);
			}

			public string MappingXmlString
			{
				get { return string.Format(XmlTemplate, idStrategy); }
			}
		}

		#endregion

		protected static readonly ILog log = LogManager.GetLogger(typeof(IdsFixture));
		protected Configuration cfg;

		[TestFixtureSetUp]
		public void TFSetup()
		{
			XmlConfigurator.Configure();
			SetIdLogLevel(Level.Debug);
		}

		[TestFixtureTearDown]
		public void TFTearDown()
		{
			SetIdLogLevel(Level.Off);			
		}

		private static void SetIdLogLevel(Level level)
		{
			ILog idlog = LogManager.GetLogger("NHibernate.Id");
			Logger logger = idlog.Logger as Logger;
			if (logger != null)
				logger.Level = level;
		}

		[SetUp]
		public void Setup()
		{
			cfg = new Configuration();
			cfg.Configure();
		}

		private void RunTest(IEntityFactory ef)
		{
			cfg.AddXml(ef.MappingXmlString);
			log.Debug("---- DDL SCHEMA ----");
			new SchemaExport(cfg).Create(true, true);
			log.Debug("--------------------");

			ISessionFactory factory = cfg.BuildSessionFactory();

			using (ISession s = factory.OpenSession())
			using (ITransaction trx = s.BeginTransaction())
			{
				for (int i = 10; i < 15; i++)
				{
					object o = ef.GetNewInstance(i);
					s.Save(o);
				}
				trx.Commit();
			}

			using (new TemporaryOffLog(new string[] { "NHibernate.SQL", "NHibernate.Transaction" }))
			using (ISession s = factory.OpenSession())
			using (ITransaction trx = s.BeginTransaction())
			{
				s.Delete("from System.Object");
				trx.Commit();
			}
			new SchemaExport(cfg).Drop(false, true);
		}

		[Test]
		public void Native()
		{
			RunTest(new AInt32Factory("native"));
		}

		[Test]
		public void Identity()
		{
			List<Type> ignoredDialects = new List<Type>();
			ignoredDialects.Add(typeof (NHibernate.Dialect.FirebirdDialect));

			if (ignoredDialects.Contains(NHibernate.Dialect.Dialect.GetDialect(cfg.Properties).GetType()))
				Assert.Ignore("Not supported by:" + NHibernate.Dialect.Dialect.GetDialect(cfg.Properties).GetType());

			RunTest(new AInt32Factory("identity"));
		}

		[Test]
		public void HighLow()
		{
			RunTest(new AInt32Factory("hilo"));
		}

		[Test]
		public void HighLowInt64()
		{
			RunTest(new AInt64Factory("hilo"));
		}

		[Test]
		public void HighLowInt64MultiSession()
		{
			List<Type> useSequenceDialects = new List<Type>();
			useSequenceDialects.Add(typeof(NHibernate.Dialect.FirebirdDialect));


				string XmlTemplate =
@"<?xml version='1.0' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
	assembly='uNHAddins.Examples.Course.Tests'
	namespace='uNHAddins.Examples.Course.Tests.Ids'>
	<class name='AInt64'>
		<id name='Id' type='long'>
			<generator class='{0}'>
				<param name='max_lo'>10</param>
			</generator>
		</id>
		<property name='Description' type='string' />
	</class>
</hibernate-mapping>";

			if (!useSequenceDialects.Contains(NHibernate.Dialect.Dialect.GetDialect(cfg.Properties).GetType()))
				cfg.AddXml(string.Format(XmlTemplate, "hilo"));
			else
				cfg.AddXml(string.Format(XmlTemplate, "seqhilo"));

			log.Debug("---- DDL SCHEMA ----");
			new SchemaExport(cfg).Create(true, true);
			log.Debug("--------------------");

			ISessionFactory factory = cfg.BuildSessionFactory();

		  log.Debug("---- First Session ----");
			using (ISession s = factory.OpenSession())
			using (ITransaction trx = s.BeginTransaction())
			{
				for (int i = 0; i < 15; i++)
				{
					AInt64 o = new AInt64("Iter="+i);
					s.Save(o);
				}
				log.Debug("---- Start Transaction Commit ----");
				trx.Commit();
				log.Debug("---- END   Transaction Commit ----");
			}

		  log.Debug("---- Second Session ----");
			using (ISession s = factory.OpenSession())
			using (ITransaction trx = s.BeginTransaction())
			{
				for (int i = 0; i < 6; i++)
				{
					AInt64 o = new AInt64("Iter=" + i);
					s.Save(o);
				}
				log.Debug("---- Start Transaction Commit ----");
				trx.Commit();
				log.Debug("---- END   Transaction Commit ----");
			}

			using (new TemporaryOffLog(new string[] { "NHibernate.SQL", "NHibernate.Transaction" }))
			using (ISession s = factory.OpenSession())
			using (ITransaction trx = s.BeginTransaction())
			{
				s.Delete("from System.Object");
				trx.Commit();
			}
			new SchemaExport(cfg).Drop(false, true);
			
		}

		[Test]
		public void Sequence()
		{
			List<Type> ignoredDialects = new List<Type>();
			ignoredDialects.Add(typeof(NHibernate.Dialect.MsSql2000Dialect));
			ignoredDialects.Add(typeof(NHibernate.Dialect.MsSql2005Dialect));

			if (ignoredDialects.Contains(NHibernate.Dialect.Dialect.GetDialect(cfg.Properties).GetType()))
				Assert.Ignore("Not supported by:" + NHibernate.Dialect.Dialect.GetDialect(cfg.Properties).GetType());

			RunTest(new AInt32Factory("sequence"));
		}

		[Test]
		public void SequenceHighLow()
		{
			List<Type> ignoredDialects = new List<Type>();
			ignoredDialects.Add(typeof(NHibernate.Dialect.MsSql2000Dialect));
			ignoredDialects.Add(typeof(NHibernate.Dialect.MsSql2005Dialect));

			if (ignoredDialects.Contains(NHibernate.Dialect.Dialect.GetDialect(cfg.Properties).GetType()))
				Assert.Ignore("Not supported by:" + NHibernate.Dialect.Dialect.GetDialect(cfg.Properties).GetType());

			RunTest(new AInt32Factory("seqhilo"));
		}

		[Test]
		public void Assigned()
		{
			RunTest(new AInt32AssignedFactory());
		}
	}
}
