using System;
using System.Linq;
using NHibernate.Cfg;
using NUnit.Framework;
using uNhAddIns.Audit;

namespace uNhAddIns.Test.Audit
{
	[TestFixture]
	public class MappingAuditableMetaDataStoreFixture
	{
		private readonly string EntityNameSimple = typeof(Simple).FullName;
		private readonly string EntityNameAddress = typeof(Address).FullName;

		[Test]
		public void CtorProtection()
		{
			Assert.Throws<ArgumentNullException>(() => new MappingAuditableMetaDataStore(null));
		}

	  [Test]
		public void CanRegister()
		{
			var conf = new Configuration().Configure();
			conf.AddResource("uNhAddIns.Test.Audit.Simple.hbm.xml", GetType().Assembly);
			IAuditableMetaDataStore store = new MappingAuditableMetaDataStore(conf);
			store.RegisterAuditableEntityIfNeeded(EntityNameSimple).Should().Be.True();
			store.GetAuditableMetaData(EntityNameSimple).Should().Not.Be.Null();
			store.Contains(EntityNameSimple).Should().Be.True();
		}

		[Test]
		[Description("Does not cause problem with not existent info.")]
		public void NotRegister()
		{
			var conf = new Configuration().Configure();
			IAuditableMetaDataStore store = new MappingAuditableMetaDataStore(conf);
			store.RegisterAuditableEntityIfNeeded("NotExists").Should().Be.False();
			store.GetAuditableMetaData("NotExists").Should().Be.Null();
			store.Contains("NotExists").Should().Be.False();

			store.RegisterAuditableEntityIfNeeded(string.Empty).Should().Be.False();
			store.GetAuditableMetaData(string.Empty).Should().Be.Null();
			store.Contains(string.Empty).Should().Be.False();
		}

		[Test]
		[Description("Should register all properties.")]
		public void RegisterProperties()
		{
			var conf = new Configuration().Configure();
			conf.AddResource("uNhAddIns.Test.Audit.Simple.hbm.xml", GetType().Assembly);
			IAuditableMetaDataStore store = new MappingAuditableMetaDataStore(conf);
			store.RegisterAuditableEntityIfNeeded(EntityNameSimple);
			var meta = store.GetAuditableMetaData(EntityNameSimple);
			meta.Propeties.Count().Should().Be.GreaterThan(0);
			meta.Propeties.First().Should().Be.EqualTo("Description");
		}

		[Test]
		[Description("Should not register properties to ignore")]
		public void NotRegisterProperties()
		{
			var conf = new Configuration().Configure();
			conf.AddResource("uNhAddIns.Test.Audit.Address.hbm.xml", GetType().Assembly);
			IAuditableMetaDataStore store = new MappingAuditableMetaDataStore(conf);
			store.RegisterAuditableEntityIfNeeded(EntityNameAddress);
			var meta = store.GetAuditableMetaData(EntityNameAddress);
			meta.Propeties.Should()
				.Contain("CivicNumber")
				.And
				.Contain("StreetName")
				.And
				.Not.Contain("Info");
		}

		[Test, Ignore("Not supported yet in NH")]
		public void CanRegisterOlySubClasses()
		{
			var conf = new Configuration().Configure();
			conf.AddResource("uNhAddIns.Test.Audit.Address.hbm.xml", GetType().Assembly);
			conf.AddResource("uNhAddIns.Test.Audit.Person.hbm.xml", GetType().Assembly);
			conf.AddResource("uNhAddIns.Test.Audit.Animal.hbm.xml", GetType().Assembly);
			IAuditableMetaDataStore store = new MappingAuditableMetaDataStore(conf);

			foreach (var s in conf.ClassMappings.Select(cm => cm.EntityName))
			{
				store.RegisterAuditableEntityIfNeeded(s);
			}

			store.Contains(typeof(Animal).FullName).Should().Be.False();
			store.Contains(typeof(Reptile).FullName).Should().Be.False();
			store.Contains(typeof(Lizard).FullName).Should().Be.False();
			store.Contains(typeof(Mammal).FullName).Should().Be.False();
			store.Contains(typeof(DomesticAnimal).FullName).Should().Be.True();
			store.Contains(typeof(Cat).FullName).Should().Be.True();
			store.Contains(typeof(Dog).FullName).Should().Be.True();
		}
	}
}