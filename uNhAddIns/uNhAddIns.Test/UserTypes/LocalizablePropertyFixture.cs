using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using NHibernate;
using NUnit.Framework;
using uNhAddIns.Criterions;
using uNhAddIns.UserTypes;

namespace uNhAddIns.Test.UserTypes
{
	[TestFixture]
	public class LocalizablePropertyFixture : TestCase
	{
		private object savedId;

		protected override IList<string> Mappings
		{
			get { return new[] {"UserTypes.EntityWithLocalizableProperty.hbm.xml"}; }
		}

		private void Cleanup()
		{
			using (ISession s = OpenSession())
			{
				s.Delete(s.Load<EntityWithLocalizableProperty>(savedId));
				s.Flush();
			}
		}

		private void FillDb()
		{
			using (ISession s = OpenSession())
			{
				savedId =
					s.Save(new EntityWithLocalizableProperty
					       	{
					       		LocalizableDescriptions =
					       			new Dictionary<CultureInfo, string>
					       				{{new CultureInfo("es-AR"), "Hola"}, {new CultureInfo("en-US"), "Hello"}}
					       	});
				s.Flush();
			}
		}

		[Test]
		public void DefaultEncloser()
		{
			// This test is to prevent a very ugly breaking-change
			LocalizablePropertyType.DefaultKeyValueEncloser.Should().Be.EqualTo('~');
		}

		[Test]
		public void SaveProperties()
		{
			FillDb();
			using (ISession s = OpenSession())
			{
				var e = s.Get<EntityWithLocalizableProperty>(savedId);
				e.LocalizableDescriptions.Should().Have.SameSequenceAs(new[]
				                                                   	{
				                                                   		new KeyValuePair<CultureInfo, string>(
				                                                   			new CultureInfo("es-AR"), "Hola"),
				                                                   		new KeyValuePair<CultureInfo, string>(
				                                                   			new CultureInfo("en-US"), "Hello")
				                                                   	});
			}
			Cleanup();
		}

		[Test]
		public void ModifyProperties()
		{
			FillDb();
			using (ISession s = OpenSession())
			{
				var e = s.Get<EntityWithLocalizableProperty>(savedId);
				e.LocalizableDescriptions.Remove(new CultureInfo("es-AR"));
				e.LocalizableDescriptions[new CultureInfo("en-US")] = "Hi!";
				s.Flush();
			}
			using (ISession s = OpenSession())
			{
				var e = s.Get<EntityWithLocalizableProperty>(savedId);
				e.LocalizableDescriptions.Should().Have.SameSequenceAs(new[]
				                                                   	{
				                                                   		new KeyValuePair<CultureInfo, string>(
				                                                   			new CultureInfo("en-US"), "Hi!")
				                                                   	});
			}
			Cleanup();
		}

		[Test]
		public void SaveNull()
		{
			using (ISession s = OpenSession())
			{
				savedId = s.Save(new EntityWithLocalizableProperty {LocalizableDescriptions = null});
				s.Flush();
			}

			using (ISession s = OpenSession())
			{
				var e = s.Get<EntityWithLocalizableProperty>(savedId);
				e.LocalizableDescriptions.Should().Be.Null();
			}
			Cleanup();
		}

		[Test]
		public void QueryWithHql()
		{
			FillDb();
			using (ISession s = OpenSession())
			{
				s.CreateQuery("from EntityWithLocalizableProperty e where e.LocalizableDescriptions like :pTemplate")
					.SetString("pTemplate", Localizable.ConvertToLikeClause("en-US", "H_l%"))
					.List()
					.Count
					.Should().Be.EqualTo(1);

				s.CreateQuery("from EntityWithLocalizableProperty e where e.LocalizableDescriptions like :pTemplate")
					.SetString("pTemplate", Localizable.ConvertToLikeClause("en-US", "H_l_"))
					.List()
					.Count
					.Should().Be.EqualTo(0);
			}
			Cleanup();
		}

		[Test]
		public void SafeParameterSetWithCriteria()
		{
			FillDb();
			using (ISession s = OpenSession())
			{
				s.CreateCriteria<EntityWithLocalizableProperty>()
					.Add(Localizable.Like("LocalizableDescriptions", "en-US", "H_ll_"))
					.List<EntityWithLocalizableProperty>().Count
					.Should().Be.EqualTo(1);
			}
			Cleanup();
		}

		[Test]
		public void CurrentCultureInfo()
		{
			FillDb();

			using (ISession s = OpenSession())
			{
				var e = s.Get<EntityWithLocalizableProperty>(savedId);

				Thread.CurrentThread.CurrentCulture = new CultureInfo("es-AR");
				e.Description.Should().Be.EqualTo("Hola");

				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
				e.Description.Should().Be.EqualTo("Hello");
			}

			Cleanup();
		}
	}
}