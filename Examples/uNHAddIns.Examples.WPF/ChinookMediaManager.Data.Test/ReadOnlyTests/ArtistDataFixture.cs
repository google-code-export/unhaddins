using ChinookMediaManager.Data.Test.BaseClasses;
using ChinookMediaManager.Domain;
using NHibernate;
using NUnit.Framework;

namespace ChinookMediaManager.Data.Test.ReadOnlyTests
{
    [TestFixture]
    public class ArtistDataFixture : ReadOnlyBaseTest
    {
        [Test]
        public void can_get_artist_1()
        {
            using (ISession session = SessionFactory.OpenSession())
            {
				var artist = session.Get<Artist>(131072);

                Assert.AreEqual("AC/DC", artist.Name);
            }
        }
    }
}