using System.ComponentModel;
using Castle.Facilities.FactorySupport;
using NHibernate;
using NUnit.Framework;
using uNhAddIns.ComponentBehaviors.Castle.Configuration;
using uNhAddIns.ComponentBehaviors.Castle.NHibernateInterceptor;
using uNhAddIns.ComponentBehaviors.Castle.Tests.SampleDomain;
using Component=Castle.MicroKernel.Registration.Component;

namespace uNhAddIns.ComponentBehaviors.Castle.Tests.NHibernateInterceptor
{
	[TestFixture]
	public class InterceptorFixture : IntegrationBaseTest
	{
		protected override void ConfigureWindsorContainer()
		{
			container.AddFacility<FactorySupportFacility>();
			container.AddFacility<ComponentBehaviorsFacility>();
			var config = new BehaviorDictionary();

			config.For<Album>().Add<NotifyPropertyChangedBehavior>().Add<EditableBehavior>();
			container.Register(Component.For<IBehaviorStore>().Instance(config));
		}

		protected override IInterceptor GetInterceptor()
		{
			return container.Resolve<ComponentBehaviorInterceptor>();
		}

		[Test]
		public void interceptor_mode_should_work()
		{
			int id;
			using (ISession s = sessions.OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				id = (int) s.Save(new Album {Title = "Hello World"});
				tx.Commit();
			}

			using (ISession s = sessions.OpenSession())
			using (s.BeginTransaction())
			{
				var album = s.Get<Album>(id);
				album.Id.Should().Be.EqualTo(id);
				album.Should().Be.AssignableTo<INotifyPropertyChanged>();
				album.Should().Be.AssignableTo<IEditableObject>();
			}
		}
	}
}