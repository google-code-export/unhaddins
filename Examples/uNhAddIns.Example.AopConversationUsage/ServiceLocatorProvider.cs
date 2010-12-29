using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.Practices.ServiceLocation;
using NHibernate;
using uNhAddIns.CastleAdapters;
using uNhAddIns.CastleAdapters.AutomaticConversationManagement;
using uNhAddIns.CastleAdapters.Tests;
using uNhAddIns.Example.AopConversationUsage.BusinessLogic;
using uNhAddIns.Example.AopConversationUsage.DataAccessObjects;
using uNhAddIns.Example.AopConversationUsage.Entities;
using uNhAddIns.SessionEasier;
using uNhAddIns.SessionEasier.Conversations;
using Castle.Facilities.FactorySupport;

namespace uNhAddIns.Example.AopConversationUsage
{
	public class ServiceLocatorProvider
	{
		public static void Initialize()
		{
			var container = new WindsorContainer();
			container.AddFacility<FactorySupportFacility>(); 
			container.AddFacility<PersistenceConversationFacility>();

			container.Register(Component.For<ISessionFactoryProvider>().ImplementedBy<SessionFactoryProvider>());
			container.Register(
				Component.For<ISessionFactory>().UsingFactoryMethod(
					() => container.Resolve<ISessionFactoryProvider>().GetFactory(null)));

			container.Register(Component.For<ISessionWrapper>().ImplementedBy<SessionWrapper>());
			container.Register(Component.For<IConversationFactory>().ImplementedBy<DefaultConversationFactory>());
			container.Register(Component.For<IConversationsContainerAccessor>().ImplementedBy<NhConversationsContainerAccessor>());

			container.Register(Component.For<IDaoFactory>().ImplementedBy<DaoFactory>());

			RegisterNaturalnessDaos<Reptile>(container);
			RegisterNaturalnessDaos<Human>(container);

			container.Register(
				Component.For(typeof (IFamilyCrudModel<>)).ImplementedBy(typeof (FamilyCrudModel<>)).LifeStyle.Transient);

			var sl = new WindsorServiceLocator(container);
			container.Register(Component.For<IServiceLocator>().Instance(sl));
			ServiceLocator.SetLocatorProvider(() => sl);
		}

		private static void RegisterNaturalnessDaos<T>(IWindsorContainer cont) where T : Animal
		{
			cont.Register(
				Component.For<IAnimalReadOnlyDao<T>, ICrudDao<T>, IDao<T>>().ImplementedBy<AnimalDao<T>>());

			cont.Register(Component.For<IFamilyDao<T>, ICrudDao<Family<T>>, IDao<Family<T>>>().ImplementedBy<FamilyDao<T>>());
		}
	}
}