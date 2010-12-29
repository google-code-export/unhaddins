using NUnit.Framework;
using uNhAddIns.WPF.Collections;
using uNhAddIns.WPF.Collections.Types;

namespace uNhAddIns.WPF.Tests.Collections
{
    [TestFixture]
    public class WpfCollectionTypeFactoryFixture
    {
        private WpfCollectionTypeFactory _colFactory;

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            _colFactory = new WpfCollectionTypeFactory();
        }

        [Test]
        public void bagofT_should_return_observablebagtypeofT()
        {
            _colFactory.Bag<string>("role","prop",true).GetType()
                .Should()
                .Be.AssignableTo(typeof (ObservableBagType<string>));
        }

        [Test]
        public void setofT_should_return_observablesettypeofT()
        {
            _colFactory.Set<string>("role","prop",true).GetType()
                .Should()
                .Be.AssignableFrom(typeof (ObservableSetType<string>));
        }

        [Test]
        public void listofT_should_return_observablelisttypeofT()
        {
            _colFactory.List<string>("role", "prop", true).GetType()
                .Should()
                .Be.AssignableFrom(typeof(ObservableListType<string>));
        }

    }

}