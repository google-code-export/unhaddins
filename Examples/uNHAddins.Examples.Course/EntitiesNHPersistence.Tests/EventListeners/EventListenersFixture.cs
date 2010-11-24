using System.Collections;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Event;
using NUnit.Framework;
using YourPrjDomain.EventListeners;

namespace uNHAddins.Examples.Course.Tests.EventListeners
{
	[TestFixture]
	public class EventListenersFixture : TestCase
	{
		protected override IList Mappings
		{
			get { return new string[] { "EventListeners.Person.hbm.xml" }; }
		}

		protected override void Configure(Configuration configuration)
		{
			cfg.SetListener(ListenerType.PreInsert, new PreSaveEL());
			cfg.SetListener(ListenerType.PostInsert, new PostSaveEL());
		}

		protected override void OnTearDown()
		{
			using (ISession s = OpenSession())
			{
				s.Delete("from Person");
				s.Flush();
			}
		}

		[Test]
		public void Save()
		{
			using (ISession s = OpenSession())
			{

				using (ITransaction tx = s.BeginTransaction())
				{
					Person p = new Person();
					p.Id = 1;
					p.FirstName = "Dario";
					p.LastName = "Quintana";

					s.Save(p);

					tx.Commit();
				}
			}
		}
	}
}