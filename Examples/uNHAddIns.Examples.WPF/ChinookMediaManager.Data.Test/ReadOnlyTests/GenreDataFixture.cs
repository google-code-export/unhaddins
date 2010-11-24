using ChinookMediaManager.Data.Test.BaseClasses;
using ChinookMediaManager.Domain;
using NHibernate;
using NUnit.Framework;
using SharpTestsEx;

namespace ChinookMediaManager.Data.Test.ReadOnlyTests
{
    [TestFixture]
    public class GenreDataFixture : ReadOnlyBaseTest
    {
        [Test]
        public void can_get_genre_1()
        {
            using (ISession session = SessionFactory.OpenSession())
            {
				var genre = session.Get<Genre>(196608);

                genre.Name.Should().Be.EqualTo("Rock");
            }
        }

    }
}