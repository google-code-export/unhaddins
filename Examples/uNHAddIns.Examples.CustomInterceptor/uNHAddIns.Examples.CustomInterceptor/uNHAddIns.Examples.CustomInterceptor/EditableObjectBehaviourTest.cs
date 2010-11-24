using System;
using Castle.Facilities.FactorySupport;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using uNHAddIns.Examples.CustomInterceptor.Domain;
using uNHAddIns.Examples.CustomInterceptor.Infrastructure;
using uNHAddIns.Examples.CustomInterceptor.Infrastructure.MethodsInterceptors;

namespace uNHAddIns.Examples.CustomInterceptor
{
    [TestFixture]
    public class EditableObjectBehaviourTest : BaseTest
    {
        //private IWindsorContainer _container;
        
        //[TestFixtureSetUp]
        //public void TestFixtureSetup()
        //{
        //    _container = new WindsorContainer();
        //    ConfigureWindsorContainer();
        //}

        protected override void ConfigureWindsorContainer()
        {
            container.AddFacility<FactorySupportFacility>();
            
            container.Register(Component.For<CommonPropertyStoreInterceptor>()
                                         .LifeStyle.Transient);

            container.Register(Component.For<PropertyChangeInterceptor>()
                                         .LifeStyle.Transient);

            container.Register(Component.For<EditableObjectInterceptor>()
                                         .LifeStyle.Transient);

            container.Register(Component.For<EntityNameInterceptor>()
                                        .LifeStyle.Transient);

            container.Register(Component.For<IProduct>()
                                        .EditableObjectBehavior()
                                        .TargetIsCommonDatastore());
        }

        [Test]
        public void property_setters_should_work()
        {
            var product = container.Resolve<IProduct>();
            product.Description = "Potatoes";
            product.Description.Should().Be.EqualTo("Potatoes");

            product.BeginEdit();
            product.Description = "Banana";
            product.Description.Should().Be.EqualTo("Banana");
            product.Description = "Apple";
            product.Description.Should().Be.EqualTo("Apple");
            product.CancelEdit();
        }

        [Test]
        public void cancel_should_discard_changes()
        {
            var product = container.Resolve<IProduct>();
            product.Description = "Potatoes";
            product.Description.Should().Be.EqualTo("Potatoes");

            product.BeginEdit();
            product.Description = "Banana";
            product.Description.Should().Be.EqualTo("Banana");
            product.CancelEdit();

            product.Description.Should().Be.EqualTo("Potatoes");

        }

        [Test]
        public void endedit_should_push_changes()
        {
            var product = container.Resolve<IProduct>();
            product.Description = "Potatoes";
            product.Description.Should().Be.EqualTo("Potatoes");

            product.BeginEdit();
            product.Description = "Banana";
            product.EndEdit();

            product.Description.Should().Be.EqualTo("Banana");

        }

        [Test]
        public void cancel_should_discard_changes_nontrascient()
        {
            var id = CreatePotatoesProduct();
            using(var session = sessions.OpenSession())
            using(var tx = session.BeginTransaction())
            {
                var product = session.Get<IProduct>(id);

                product.Description.Should().Be.EqualTo("Potatoes");

                product.BeginEdit();
                product.Description = "Banana";
                product.Description.Should().Be.EqualTo("Banana");
                product.CancelEdit();

                product.Description.Should().Be.EqualTo("Potatoes");
            }
        }

        [Test]
        public void endedit_should_push_changes_nontrascient()
        {
            var id = CreatePotatoesProduct();
            using (var session = sessions.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var product = session.Get<IProduct>(id);

                product.Description.Should().Be.EqualTo("Potatoes");

                product.BeginEdit();
                product.Description = "Banana";
                product.Description.Should().Be.EqualTo("Banana");
                product.EndEdit();
                product.Description.Should().Be.EqualTo("Banana");
            }
        }

        private Guid CreatePotatoesProduct()
        {
            Guid id;
            using (var session = sessions.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var potatoes = ServiceLocator.Current.GetInstance<IProduct>();

                potatoes.Description = "Potatoes";
                potatoes.UnitPrice = 2;

                id = (Guid)session.Save(potatoes);
                tx.Commit();
                session.Clear();
            }
            return id;
        }


    }
}