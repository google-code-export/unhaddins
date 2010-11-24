using System.ComponentModel;
using Castle.Facilities.FactorySupport;
using NHibernate;
using NUnit.Framework;
using uNhAddIns.ComponentBehaviors.Castle.Configuration;
using uNhAddIns.ComponentBehaviors.Castle.Tests.SampleDomain;
using Component=Castle.MicroKernel.Registration.Component;

namespace uNhAddIns.ComponentBehaviors.Castle.Tests
{
	[TestFixture]
	public class PropertyChangeNotifierFixture : IntegrationBaseTest
	{
		protected override void ConfigureWindsorContainer()
		{
			container.AddFacility<FactorySupportFacility>();
			container.AddFacility<ComponentBehaviorsFacility>();

			var config = new BehaviorDictionary();
			config.For<Album>().Add<NotifyPropertyChangedBehavior>();
			container.Register(Component.For<IBehaviorStore>().Instance(config));

			container.Register(Component.For<Album>().LifeStyle.Transient);
		}


		int CreateNewAlbum()
		{
			int id;
			using (ISession session = sessions.OpenSession())
			using (ITransaction tx = session.BeginTransaction())
			{
				var album = new Album();
				album.Title = "The dark side of the moon";
				id = (int) session.Save(album);
				tx.Commit();
			}
			return id;
		}


		[Test]
		public void can_raise_propertychanged()
		{
			bool eventWasRaised = false;

			var album = container.Resolve<Album>();
			((INotifyPropertyChanged) album).PropertyChanged +=
				(sender, e) =>
					{
						eventWasRaised = true;
						e.PropertyName.Should().Be.EqualTo("Title");
					};

			album.Title = "dark side";
			eventWasRaised.Should().Be.True();
		}

		[Test]
		public void can_raise_propertychanged_in_nontransientobject()
		{
			int id = CreateNewAlbum();
			bool eventWasRaised = false;

			using (ISession session = sessions.OpenSession())
			{
				var album = session.Get<Album>(id);

				((INotifyPropertyChanged) album).PropertyChanged +=
					(sender, e) =>
						{
							eventWasRaised = true;
							e.PropertyName.Should().Be.EqualTo("Title");
						};

				album.Title = "dark side";

				eventWasRaised.Should().Be.True();
			}
		}
	}
}