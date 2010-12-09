using System;
using System.Collections.Generic;
using NHibernate.SqlTypes;
using NUnit.Framework;
using uNhAddIns.TestUtils.NhIntegration;
using uNhAddIns.UserTypes;

namespace uNhAddIns.Test.UserTypes
{
	public class GenderType : GenericWellKnownInstanceType<Gender, string>
	{
        public GenderType() : base(new Genders(), (e, k) => e.Id == k, e => e.Id) { }

	    /// <summary>
	    /// The SQL types for the columns mapped by this type. 
	    /// </summary>
	    public override SqlType[] SqlTypes
	    {
            get { return new[]{SqlTypeFactory.GetString(1)}; }
	    }
	}

	[TestFixture]
	public class GenericWellKnownInstanceTypeFixture : TestCase
	{
		protected override IList<string> Mappings
		{
			get { return new[] { "UserTypes.PersonMitaMita.hbm.xml" }; }
		}

		[Test]
		public void Crud()
		{
			object savedId = null;
			sessions.EncloseInTransaction(s => savedId = s.Save(new PersonMitaMita { Name = "Fabio", Gender = Genders.Male}));
			PersonMitaMita person = null;
			sessions.EncloseInTransaction(s => person = s.Get<PersonMitaMita>(savedId));
			person.Gender.Should().Be.EqualTo(Genders.Male);

			sessions.EncloseInTransaction(s =>
											{
												var u = s.Get<PersonMitaMita>(savedId);
											    u.Gender = Genders.Female;
											});

			sessions.EncloseInTransaction(s => person = s.Get<PersonMitaMita>(savedId));
			person.Gender.Should().Be.EqualTo(Genders.Female);
			sessions.EncloseInTransaction(s => s.Delete(s.Load<PersonMitaMita>(savedId)));
		}

		[Test]
		public void Queries()
		{
			sessions.EncloseInTransaction(s =>
			{
				s.Save(new PersonMitaMita { Name = "Fabio", Gender = Genders.Male });
				s.Save(new PersonMitaMita { Name = "Maulo", Gender = Genders.Female});
			});

			sessions.EncloseInTransaction(s =>
			{
				var l = s.CreateQuery("from PersonMitaMita u where u.Gender in (:genders)")
					.SetParameterList("genders", new[] { Genders.Female })
					.List<PersonMitaMita>();
				l.Should().Have.Count.EqualTo(1);
			});
			sessions.EncloseInTransaction(s =>
			{
				var l = s.CreateQuery("from PersonMitaMita u where u.Gender = :Gender")
					.SetParameter("Gender", (object)Genders.Male)
					.List<PersonMitaMita>();
				l.Should().Have.Count.EqualTo(1);
				l[0].Name.Should().Be.EqualTo("Fabio");
			});
			sessions.EncloseInTransaction(s => s.CreateQuery("delete from PersonMitaMita").ExecuteUpdate());
		}
	}
}