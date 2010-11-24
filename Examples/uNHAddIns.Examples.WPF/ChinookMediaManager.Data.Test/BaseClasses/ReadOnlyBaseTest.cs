using NHibernate;
using NHibernate.Cfg;
using NUnit.Framework;

namespace ChinookMediaManager.Data.Test.BaseClasses
{
    [TestFixture]
    public abstract class ReadOnlyBaseTest
    {
        protected ISessionFactory SessionFactory;

        [TestFixtureSetUp]
        public void SetUp()
        {
            var cfg = new Configuration();
            cfg.Configure("hibernate-readonly.cfg.xml");
            SessionFactory = cfg.BuildSessionFactory();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            SessionFactory.Close();
        }
    }
}