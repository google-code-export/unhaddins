using System.ComponentModel;
using Castle.Facilities.FactorySupport;
using Castle.Windsor;
using Moq;
using NUnit.Framework;
using uNhAddIns.Adapters;
using uNhAddIns.ComponentBehaviors.Castle.Configuration;
using uNhAddIns.ComponentBehaviors.Castle.ProxyFactory;
using uNhAddIns.ComponentBehaviors.Castle.Tests.SampleDomain;
using uNhAddIns.TestUtils.NhIntegration;
using Component=Castle.MicroKernel.Registration.Component;

namespace uNhAddIns.ComponentBehaviors.Castle.Tests.ProxyFactory
{
	[TestFixture]
	public class ProxyFactoryTest : IntegrationBaseTest
	{
		protected override void ConfigureWindsorContainer()
		{
			container.AddFacility<FactorySupportFacility>();
			container.AddFacility<ComponentBehaviorsFacility>();

			//container.Register(Component.For<Album>().LifeStyle.Transient);
			//register the proxyfactoryfactory.
			container.Register(Component.For<IEntityValidator>().Instance(new Mock<IEntityValidator>().Object));

			//configure the behavior metadata
			var config = new BehaviorDictionary();
			config.For<Album>().Add<NotifyPropertyChangedBehavior>().Add<DataErrorInfoBehavior>();
			container.Register(Component.For<IBehaviorStore>().Instance(config));

		}

		protected override string GetProxyFactoryFactory()
		{
			//this fixture uses the following proxyfactoryfactory
			return typeof (ComponentProxyFactoryFactory).AssemblyQualifiedName;
		}

		[Test]
		public void equal_should_work()
		{
			int id = 0;
			sessions.EncloseInTransaction(s => { id = (int) s.Save(new Album {Title = "a"}); });

			using (var s = sessions.OpenSession())
			using (var tx = s.BeginTransaction())
			{
				var albumProxy = s.Load<Album>(id);
				var album = s.Get<Album>(id);


				album.Should().Be.EqualTo(albumProxy);
				albumProxy.Should().Be.EqualTo(album);
				tx.Commit();
			}
		}

		[Test]
		public void load_should_work()
		{
			int id = 0;
			sessions.EncloseInTransaction(s => { id = (int) s.Save(new Album {Title = "a"}); });

			using (var s = sessions.OpenSession())
			using (var tx = s.BeginTransaction())
			{
				var album = s.Load<Album>(id);
				album.Should().Be.AssignableTo<INotifyPropertyChanged>();
				album.Should().Be.AssignableTo<IDataErrorInfo>();

				//simple test
				bool eventWasRaised = false;
				((INotifyPropertyChanged) album).PropertyChanged += (sender, args) =>
					{
						if (args.PropertyName.Equals("Title"))
							eventWasRaised = true;
					};

				album.Title = "new title";
				eventWasRaised.Should().Be.True();
				//

				tx.Commit();
			}
		}
	}
}