using System.Collections.Generic;
using NHibernate;
using NUnit.Framework;

namespace uNhAddIns.Test.UserTypes
{
    [TestFixture]
	public class TrimmedStringFixture : TestCase
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
        public void description_is_trimmed()
        {
        	int userId;
        	var user = new User
        	            	{
        	            		Name = "Astor Piazolla", 
								Description = "1000  ", 
								TrimmedDescription = "1000  "
        	            	};
        	using (ISession s = OpenSession())
            {
                s.Save(user);
                s.Flush();
            	userId = user.Id;
            }

            using (ISession s = OpenSession())
            {
				User trimmedUser = s.Get<User>(userId);

				Assert.That(trimmedUser.Description.Length, Is.EqualTo(32));
				Assert.That(trimmedUser.TrimmedDescription.Length, Is.EqualTo(4));
            }
        }
    }
}