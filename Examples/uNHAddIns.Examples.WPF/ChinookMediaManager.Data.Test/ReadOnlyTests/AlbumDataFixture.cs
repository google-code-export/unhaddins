using System.Linq;
using ChinookMediaManager.Data.Test.BaseClasses;
using ChinookMediaManager.Domain;
using NHibernate;
using NUnit.Framework;
using SharpTestsEx;

namespace ChinookMediaManager.Data.Test.ReadOnlyTests
{
    [TestFixture]
    public class AlbumDataFixture : ReadOnlyBaseTest
    {
        [Test]
        public void CanGetAlbum1()
        {
            using (ISession session = SessionFactory.OpenSession())
            {
				var album = session.Get<Album>(65536);
				album.Artist.Id.Should().Be.EqualTo(131072);
                album.Title.Should().Be.EqualTo("For Those About To Rock We Salute You");
                album.Tracks.Count.Should().Be.EqualTo(10);
                album.Tracks.First().Name.Should().Be.EqualTo("For Those About To Rock (We Salute You)");
				album.Tracks.First().MediaType.Id.Should().Be.EqualTo(294912);
                album.Tracks.First().Composer.Should().Be.EqualTo("Angus Young, Malcolm Young, Brian Johnson");
            }
        }
    }
}