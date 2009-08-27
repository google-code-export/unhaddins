using System.ComponentModel;
using Castle.Core;
using Castle.Windsor;
using NUnit.Framework;
using uNhAddIns.ComponentBehaviors.Castle.EntityNameResolver;
using uNhAddIns.ComponentBehaviors.Castle.Tests.SampleDomain;
using Component=Castle.MicroKernel.Registration.Component;

namespace uNhAddIns.ComponentBehaviors.Castle.Tests
{
    [TestFixture]
    public class EditableNotifyIntegrationFixture //: IntegrationBaseTest
    {
        private readonly IWindsorContainer _container = new WindsorContainer();

        [TestFixtureSetUp]
        protected void ConfigureWindsorContainer()
        {
            _container.Register(Component.For<EditableBehaviorInterceptor>()
                                    .LifeStyle.Transient);

            _container.Register(Component.For<GetEntityNameInterceptor>()
                                    .LifeStyle.Transient);

            _container.Register(Component.For<PropertyChangedInterceptor>().LifeStyle.Transient);
            _container.Register(Component.For<Album>()
                                    .Proxy.AdditionalInterfaces(typeof (IEditableObject), typeof (INamedEntity),
                                                                typeof (INotifyPropertyChanged))
                                    .Interceptors(new InterceptorReference(typeof (PropertyChangedInterceptor))).First
                                    .Interceptors(new InterceptorReference(typeof (GetEntityNameInterceptor))).Anywhere
                                    .Interceptors(new InterceptorReference(typeof (EditableBehaviorInterceptor))).Last
                                    .LifeStyle.Transient);
        }

        [Test]
        public void canceledit_should_notify_for_each_restored_property()
        {
            var album = _container.Resolve<Album>();
            int eventTimes = 0;
            ((INotifyPropertyChanged) album).PropertyChanged += (sender, e) =>
                                                                    {
                                                                        eventTimes++;
                                                                        e.PropertyName.Should().Be.EqualTo("Title");
                                                                    };

            album.Title = "dark";
            ((IEditableObject) album).BeginEdit();
            album.Title = "dark side of the moon";
            ((IEditableObject) album).CancelEdit();

            eventTimes.Should().Be.EqualTo(3);
        }
    }
}