using System;
using NHibernate;
using NUnit.Framework;
using YourPrjDomain.Ranges;

namespace uNHAddins.Examples.Course.Tests.Core
{
	[TestFixture]
	public class ComponentFixture : TestCase
	{
		protected override System.Collections.IList Mappings
		{
			get { return new string[] { "Ranges.Account.hbm.xml" }; }
		}

		[Test]
		public void CR()
		{
			object savedId;
			Account a = new Account();
			a.Life.LowLimit = DateTime.Now.AddDays(-2);
			a.Life.HighLimit = DateTime.Now;
			log.DebugFormat("TimeSpan = {0}", a.Life.Gap());
			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				s.Save(a);
				savedId = a.Id;
				t.Commit();
			}

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				Account aa = s.Get<Account>(savedId);
				s.Delete(aa);
				t.Commit();
			}
		}
	}
}
