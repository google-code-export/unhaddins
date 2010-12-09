using Castle.Facilities.FactorySupport;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using uNHAddIns.Examples.CustomInterceptor.Domain;
using uNHAddIns.Examples.CustomInterceptor.Infrastructure;
using uNHAddIns.Examples.CustomInterceptor.Infrastructure.MethodsInterceptors;

namespace uNHAddIns.Examples.CustomInterceptor
{
    [TestFixture]
    public class CommonPropertyStoreTest
    {
        private IWindsorContainer _container;

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            _container = new WindsorContainer();
            ConfigureWindsorContainer();
        }

        private void ConfigureWindsorContainer()
        {
            _container.AddFacility<FactorySupportFacility>();

            _container.Register(Component.For<CommonPropertyStoreInterceptor>()
                                         .LifeStyle.Transient);

            _container.Register(Component.For<IProduct>()
                                        .TargetIsCommonDatastore().LifeStyle.Transient);
        }

        [Test]
        public void property_setters_should_work()
        {
            var product = _container.Resolve<IProduct>();
            
            product.Description = "Potatoes";
            product.Description.Should().Be.EqualTo("Potatoes");

            //product.BeginEdit();
            product.Description = "Banana";
            product.Description.Should().Be.EqualTo("Banana");
            product.Description = "Apple";
            product.Description.Should().Be.EqualTo("Apple");
            //product.CancelEdit();
        }
        [Test]
        public void property_getters_should_return_defaults_with_valuetypes()
        {
            var product = _container.Resolve<IProduct>();

            product.Description.Should().Be.EqualTo(string.Empty);

            product.UnitPrice.Should().Be.EqualTo((decimal)0);
        }



    }
}