using System.Collections.Generic;
using NHibernate;
using NUnit.Framework;

namespace uNhAddIns.Test.UserTypes
{
	[TestFixture]
	public class EncryptedStringFixture : TestCase
	{
		protected override IList<string> Mappings
		{
			get { return new[] { "UserTypes.User.hbm.xml" }; }
		}

		protected override void OnTearDown()
		{
			using (ISession s = OpenSession())
			{
				s.Delete("from User");
				s.Flush();
			}
		}

		[Test]
		public void SaveEncryptedPasswords()
		{
			var user = new User { Id = 1, Name = "Astor Piazolla", Password = "tango", OtherEncripted = "Adios Nonino" };

			using (ISession s = OpenSession())
			{
				s.Save(user);
				s.Flush();
			}

			using (ISession s = OpenSession())
			{
				var decryptedUser = s.Get<User>(1);

				Assert.AreEqual(1, decryptedUser.Id);
				Assert.AreEqual("Astor Piazolla", decryptedUser.Name);
				Assert.AreEqual("tango", decryptedUser.Password);
				Assert.That(decryptedUser.OtherEncripted, Is.EqualTo("Adios Nonino"));
			}
		}

		[Test]
		public void SaveNullPropertyAndGetItBack()
		{
			var user = new User { Id = 2, Name = "Pat Metheny", Password = null };

			using (ISession s = OpenSession())
			{
				s.Save(user);
				s.Flush();
			}

			using (ISession s = OpenSession())
			{
				var decryptedUser = s.Get<User>(2);

				Assert.AreEqual(2, decryptedUser.Id);
				Assert.AreEqual("Pat Metheny", decryptedUser.Name);
				Assert.IsNull(decryptedUser.Password);
			}
		}
	}
}