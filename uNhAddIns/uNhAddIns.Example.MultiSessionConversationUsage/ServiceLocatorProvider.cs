using Castle.Facilities.FactorySupport;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.Practices.ServiceLocation;
using uNhAddIns.CastleAdapters.Tests;
using uNhAddIns.Example.MultiSessionConversationUsage.BusinessLogic;
using uNhAddIns.Example.MultiSessionConversationUsage.DataAccessObjects;
using NHibernate;
using uNhAddIns.CastleAdapters;
using uNhAddIns.CastleAdapters.AutomaticConversationManagement;
using uNhAddIns.SessionEasier;
using uNhAddIns.SessionEasier.Conversations;

namespace uNhAddIns.Example.MultiSessionConversationUsage
{
	public class ServiceLocatorProvider
	{
		private const string sessionFactoryProviderComponentKey = "sessionFactoryProvider";

		public static void Initialize()
		{
			var container = new WindsorContainer();
			container.AddFacility<FactorySupportFacility>(); 
			container.AddFacility<PersistenceConversationFacility>();

			container.Register(Component
				.For<ISessionFactoryProvider>()
				.Named(sessionFactoryProviderComponentKey)
				.ImplementedBy<MultiSessionFactoryProvider>());

			RegisterSessionFactoryFor("uNhAddIns1", container);
			RegisterSessionFactoryFor("uNhAddIns2", container);

			container.Register(Component.For<ISessionWrapper>().ImplementedBy<SessionWrapper>());
			container.Register(Component.For<IConversationFactory>().ImplementedBy<DefaultConversationFactory>());
			container.Register(Component.For<IConversationsContainerAccessor>().ImplementedBy<NhConversationsContainerAccessor>());

			container.Register(Component.For<IPersonDao>().ImplementedBy<PersonDao>().Parameters(Parameter.ForKey("factory")
									.Eq("${" + GetSessionFactoryProviderKey("uNhAddIns2") + "}")));

			container.Register(Component.For<IAnimalDao>().ImplementedBy<AnimalDao>().Parameters(Parameter.ForKey("factory")
									.Eq("${" + GetSessionFactoryProviderKey("uNhAddIns1") + "}")));

			container.Register(Component.For<IPersonCrudModel>().ImplementedBy<PersonCrudModel>());
			container.Register(Component.For<IAnimalCrudModel>().ImplementedBy<AnimalCrudModel>());

			var sl = new WindsorServiceLocator(container);
			container.Register(Component.For<IServiceLocator>().Instance(sl));
			ServiceLocator.SetLocatorProvider(() => sl);
		}

		private static void RegisterSessionFactoryFor(string sessionFactoryName, IWindsorContainer windsorContainer)
		{
			windsorContainer.Register(
				Component.For<ISessionFactory>()
				.Named(GetSessionFactoryProviderKey(sessionFactoryName))
				.Configuration(Attrib.ForName("factoryId").Eq(sessionFactoryProviderComponentKey),
					Attrib.ForName("factoryCreate").Eq("GetFactory"))
					.Parameters(Parameter.ForKey("factoryId").Eq(sessionFactoryName)));
		}

		private static string GetSessionFactoryProviderKey(string sessionFactoryName)
		{
			return sessionFactoryName + ".sessionFactory";
		}

	}
}