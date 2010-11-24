using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using Castle.Core;
using Castle.Facilities.FactorySupport;
using NUnit.Framework;
using uNHAddIns.Examples.CustomInterceptor.Domain;
using uNHAddIns.Examples.CustomInterceptor.Infrastructure;
using uNHAddIns.Examples.CustomInterceptor.Infrastructure.MethodsInterceptors;
using Component=Castle.MicroKernel.Registration.Component;

namespace uNHAddIns.Examples.CustomInterceptor
{
    [TestFixture]
    public class NotifyCollectionChangeTest : BaseTest
    {
        protected override void ConfigureWindsorContainer()
        {
            container.AddFacility<FactorySupportFacility>();

            container.Register(Component.For<CommonPropertyStoreInterceptor>()
                                   .LifeStyle.Transient);

            container.Register(Component.For<EntityNameInterceptor>()
                                   .LifeStyle.Transient);

            container.Register(Component.For<CollectionPropertyInterceptor>()
                                   .LifeStyle.Transient);

            container.Register(Component.For<PropertyChangeInterceptor>()
                                   .LifeStyle.Transient);

container.Register(Component.For<Store>()
                       .Interceptors(new InterceptorReference(
                           typeof (CollectionPropertyInterceptor))).Anywhere
                       .ImplementedBy<Store>()
                       .EnableNhibernateEntityCompatibility()
                       .LifeStyle.Transient);

container.Register(Component.For<IStore>()
                       .UsingFactoryMethod(kernel => kernel.Resolve<Store>())
                       .LifeStyle.Transient);

            container.Register(Component.For<IProduct>()
                                   .TargetIsCommonDatastore().LifeStyle.Transient);
        }

        private static IProduct CreateNewProduct()
        {
            return container.Resolve<IProduct>();
        }

        [Test]
        public void NotifyOnChangeProperty_Should_Implement_INotifyCollectionChanged()
        {
            var store = container.Resolve<IStore>();
            typeof(INotifyCollectionChanged).IsAssignableFrom(store.Products.GetType());
            typeof(INotifyPropertyChanged).IsAssignableFrom(store.Products.GetType());
        }

        [Test]
        public void add_should_raise_collectionchanged()
        {
            var store = container.Resolve<IStore>();
            IProduct newProduct = CreateNewProduct();
            bool eventWasRaised = false;

            ((INotifyCollectionChanged) store.Products)
                .CollectionChanged += (sender, args) =>
                {
                    eventWasRaised = true;
                    args.Action.Should().Be.EqualTo(NotifyCollectionChangedAction.Add);
                    args.NewItems.Count.Should().Be.EqualTo(1);
                    args.NewItems[0].Should().Be.EqualTo(newProduct);
                };


            store.AddProduct(newProduct);

            eventWasRaised.Should().Be.True();
        }

        [Test]
        public void add_should_raise_propertychange_for_count()
        {
            var store = container.Resolve<IStore>();
            IProduct newProduct = CreateNewProduct();
            bool eventWasRaised = false;

            ((INotifyPropertyChanged)store.Products)
                .PropertyChanged += (sender, args) =>
                    {
                        if(args.PropertyName == "Count")
                            eventWasRaised = true;
                    };

            store.AddProduct(newProduct);
            eventWasRaised.Should().Be.True();
        }

        [Test]
        public void add_should_raise_propertychange_for_item()
        {
            var store = container.Resolve<IStore>();
            IProduct newProduct = CreateNewProduct();
            bool eventWasRaised = false;

            ((INotifyPropertyChanged)store.Products)
                .PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == "Item[]")
                        eventWasRaised = true;
                };

            store.AddProduct(newProduct);
            eventWasRaised.Should().Be.True();
        }
        
        [Test]
        public void clear_should_raise_collectionchanged()
        {
            var store = container.Resolve<IStore>();
            IProduct newProduct1 = CreateNewProduct();
            IProduct newProduct2 = CreateNewProduct();

            bool eventWasRaised = false;

            store.AddProduct(newProduct1);
            store.AddProduct(newProduct2);

            ((INotifyCollectionChanged) store.Products)
                .CollectionChanged += (sender, args) =>
                                          {
                                              eventWasRaised = true;
                                              args.Action.Should().Be.EqualTo(NotifyCollectionChangedAction.Reset);
                                          };


            store.RemoveAllProducts();
            eventWasRaised.Should().Be.True();
        }

        [Test]
        public void insert_should_raise_collectionchanged()
        {
            var store = container.Resolve<IStore>();
            IProduct product1 = CreateNewProduct();
            IProduct product2 = CreateNewProduct();
            IProduct product3 = CreateNewProduct();

            bool eventWasRaised = false;

            store.AddProduct(product1);
            store.AddProduct(product2);

            ((INotifyCollectionChanged) store.Products)
                .CollectionChanged += (sender, args) =>
                                          {
                                              eventWasRaised = true;
                                              args.Action.Should().Be.EqualTo(NotifyCollectionChangedAction.Add);
                                              args.NewStartingIndex.Should().Be.EqualTo(1);
                                              args.NewItems.Count.Should().Be.EqualTo(1);
                                              args.NewItems[0].Should().Be.EqualTo(product3);
                                          };


            store.InsertProduct(1, product3);
            eventWasRaised.Should().Be.True();
        }

        [Test]
        public void remove_should_raise_collectionchanged()
        {
            var store = container.Resolve<IStore>();
            IProduct newProduct = CreateNewProduct();
            bool eventWasRaised = false;

            store.AddProduct(newProduct);

            ((INotifyCollectionChanged) store.Products)
                .CollectionChanged += (sender, args) =>
                                          {
                                              eventWasRaised = true;
                                              args.Action.Should().Be.EqualTo(NotifyCollectionChangedAction.Remove);
                                              args.OldItems.Count.Should().Be.EqualTo(1);
                                              args.OldItems[0].Should().Be.EqualTo(newProduct);
                                          };


            store.RemoveProduct(newProduct);
            eventWasRaised.Should().Be.True();
        }

        [Test]
        public void removeat_should_raise_collectionchanged()
        {
            var store = container.Resolve<IStore>();
            IProduct newProduct = CreateNewProduct();
            bool eventWasRaised = false;

            store.AddProduct(newProduct);

            ((INotifyCollectionChanged) store.Products)
                .CollectionChanged += (sender, args) =>
                                          {
                                              eventWasRaised = true;
                                              args.Action.Should().Be.EqualTo(NotifyCollectionChangedAction.Remove);
                                              args.OldItems.Count.Should().Be.EqualTo(1);
                                              args.OldItems[0].Should().Be.EqualTo(newProduct);
                                          };


            store.RemoveProductAt(0);
            eventWasRaised.Should().Be.True();
        }


        [Test]
        public void add_should_raise_collectionchanged_on_nontrascient()
        {
            var id = CreateFooStore();
            IProduct newProduct = CreateNewProduct();

            using (var session = sessions.OpenSession())
            using(session.BeginTransaction())
            {
                var store = session.Get<Store>(id);
                bool eventWasRaised = false;
                ((INotifyCollectionChanged)store.Products)
                    .CollectionChanged += (sender, args) =>
                        {
                            eventWasRaised = true;
                            
                            args.Action
                                .Should().Be.EqualTo(NotifyCollectionChangedAction.Add);
                            
                            args.NewItems.Count
                                .Should().Be.EqualTo(1);
                            
                            args.NewItems[0]
                                .Should().Be.EqualTo(newProduct);
                        };


                store.AddProduct(newProduct);

                eventWasRaised.Should().Be.True();
            }

        }


        private Guid CreateFooStore()
        {
            Guid id;
            using (var session = sessions.OpenSession())
            using(var tx = session.BeginTransaction())
            {
                var store = container.Resolve<Store>();
                store.Name = "Foo store";
                store.AddProduct(CreateNewProduct());
                
                id = (Guid)session.Save(store);
                tx.Commit();
            }
            return id;
        } 
    }
}