using ChinookMediaManager.Data.Test.BaseClasses;
using ChinookMediaManager.Domain;
using NHibernate;
using NUnit.Framework;
using SharpTestsEx;

namespace ChinookMediaManager.Data.Test.ReadOnlyTests
{
    [TestFixture]
    public class MediaTypeDataFixture : ReadOnlyBaseTest
    {
        [Test]
        public void can_get_mediatype_1()
        {
            using (ISession session = SessionFactory.OpenSession())
            {
				var mediaType = session.Get<MediaType>(294912);
                mediaType.Name.Should().Be.EqualTo("MPEG audio file");
            }
        }
    }
}