using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Castle.Core;
using Castle.Windsor;
using NUnit.Framework;
using uNhAddIns.ComponentBehaviors.Castle.Tests.SampleDomain;
using uNhAddIns.NHibernateTypeResolver;
using Component=Castle.MicroKernel.Registration.Component;

namespace uNhAddIns.ComponentBehaviors.Castle.Tests.Fixes
{
    [TestFixture]
    public class EditableNotifyIntegrationFixture //: IntegrationBaseTest
    {
        private readonly IWindsorContainer _container = new WindsorContainer();

        [TestFixtureSetUp]
        protected void ConfigureWindsorContainer()
        {
            _container.Register(Component.For<EditableBehavior>()
                                    .LifeStyle.Transient);

            _container.Register(Component.For<GetEntityNameBehavior>()
                                    .LifeStyle.Transient);

            _container.Register(Component.For<NotifyPropertyChangedBehavior>().LifeStyle.Transient);
            _container.Register(Component.For<Album>()
                                    .Proxy.AdditionalInterfaces(typeof (IEditableObject), typeof (IWellKnownProxy),
                                                                typeof (INotifyPropertyChanged))
                                    .Interceptors(new InterceptorReference(typeof (NotifyPropertyChangedBehavior))).First
                                    .Interceptors(new InterceptorReference(typeof (GetEntityNameBehavior))).Anywhere
                                    .Interceptors(new InterceptorReference(typeof (EditableBehavior))).Last
                                    .LifeStyle.Transient);
        }

        [Test]
        public void canceledit_should_notify_for_each_property()
        {
            var album = _container.Resolve<Album>();
            
			var writtablePropertiesCount = album.GetType()
												.GetProperties(BindingFlags.Public | BindingFlags.Instance)
												.Where(p => p.CanWrite)
												.Select(p => p.Name)
												.ToArray();
        	
			var hashedSet = new HashSet<string>();

            ((INotifyPropertyChanged) album).PropertyChanged += (sender, e) => hashedSet.Add(e.PropertyName);

			((IEditableObject) album).BeginEdit();
            ((IEditableObject) album).CancelEdit();

			writtablePropertiesCount.All(hashedSet.Contains).Should().Be.True();
        	
        }
    }
}