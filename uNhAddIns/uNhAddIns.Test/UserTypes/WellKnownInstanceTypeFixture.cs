using System.Collections.Generic;
using NUnit.Framework;
using uNhAddIns.TestUtils.NhIntegration;
using uNhAddIns.UserTypes;

namespace uNhAddIns.Test.UserTypes
{
	public class CountryType : WellKnownInstanceType<Country>
	{
		public CountryType() : base(new Countries(), (e, k) => e.Id == k, e => e.Id) {}
	}

	[TestFixture]
	public class WellKnownInstanceTypeFixture : TestCase
	{
		protected override IList<string> Mappings
		{
			get { return new[] { "UserTypes.UserMitaMita.hbm.xml" }; }
		}

		[Test]
		public void Crud()
		{
			object savedId = null;
			sessions.EncloseInTransaction(s => savedId = s.Save(new UserMitaMita { Name = "Fabio", Country = Countries.Argentina }));
			UserMitaMita user = null;
			sessions.EncloseInTransaction(s => user = s.Get<UserMitaMita>(savedId));
			user.Country.Should().Be.EqualTo(Countries.Argentina);

			sessions.EncloseInTransaction(s =>
																			{
																				var u = s.Get<UserMitaMita>(savedId);
																				u.Country = Countries.Italy;
																			});

			sessions.EncloseInTransaction(s => user = s.Get<UserMitaMita>(savedId));
			user.Country.Should().Be.EqualTo(Countries.Italy);
			sessions.EncloseInTransaction(s => s.Delete(s.Load<UserMitaMita>(savedId)));
		}

		[Test]
		public void Queries()
		{
			sessions.EncloseInTransaction(s =>
			{
				s.Save(new UserMitaMita { Name = "Fabio", Country = Countries.Argentina });
				s.Save(new UserMitaMita { Name = "Maulo", Country = Countries.Italy });
			});

			sessions.EncloseInTransaction(s =>
			{
				var l = s.CreateQuery("from UserMitaMita u where u.Country in (:countries)")
					.SetParameterList("countries", new[] { Countries.Argentina, Countries.Italy })
					.List<UserMitaMita>();
				l.Should().Have.Count.EqualTo(2);
			});
			sessions.EncloseInTransaction(s =>
			{
				var l = s.CreateQuery("from UserMitaMita u where u.Country = :country")
					.SetParameter("country", (object)Countries.Argentina)
					.List<UserMitaMita>();
				l.Should().Have.Count.EqualTo(1);
				l[0].Name.Should().Be.EqualTo("Fabio");
			});
			sessions.EncloseInTransaction(s => s.CreateQuery("delete from UserMitaMita").ExecuteUpdate());
		}
	}
}