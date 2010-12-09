using NHibernate;
using NHibernate.Tool.hbm2ddl;
using uNhAddIns.Example.ConversationUsage.BusinessLogic;
using uNhAddIns.Example.ConversationUsage.DataAccessObjects;
using uNhAddIns.Example.ConversationUsage.Entities;
using uNhAddIns.Example.ConversationUsage.MultiTiers;
using uNhAddIns.SessionEasier;
using uNhAddIns.SessionEasier.Conversations;

namespace uNhAddIns.Example.ConversationUsage
{
	public class ServiceLocatorProvider
	{
		public IServiceLocator GetServiceLocator()
		{
			var sl = new HomeMadeServiceLocator();

			var nhConfigurator = new DefaultSessionFactoryConfigurationProvider();
			nhConfigurator.AfterConfigure += ((sender, e) => new SchemaExport(e.Configuration).Create(false, true));
			var sfp = new SessionFactoryProvider(nhConfigurator);
			sl.LoadSingletonService<ISessionFactoryProvider>(sfp);

			sl.LoadSingletonService<IConversationFactory>(new DefaultConversationFactory(sfp, new FakeSessionWrapper()));

			sl.LoadSingletonService<IConversationsContainerAccessor>(new NhConversationsContainerAccessor(sfp));

			sl.LoadSingletonService<IDaoFactory>(new DaoFactory(sl));
			sl.LoadDelegatedService(() => sfp.GetFactory(null));
			LoadNaturalnessDaos<Reptile>(sl);
			LoadNaturalnessDaos<Human>(sl);
			LoadNaturalnessModels<Reptile>(sl);
			LoadNaturalnessModels<Human>(sl);
			return sl;
		}

		private static void LoadNaturalnessDaos<T>(HomeMadeServiceLocator sl) where T : Animal
		{
			var animalDao = new AnimalDao<T>(sl.Resolve<ISessionFactory>());
			sl.LoadSingletonService<IAnimalReadOnlyDao<T>>(animalDao);
			sl.LoadSingletonService<ICrudDao<T>>(animalDao);
			sl.LoadSingletonService<IDao<T>>(animalDao);

			var familyDao = new FamilyDao<T>(sl.Resolve<ISessionFactory>());
			sl.LoadSingletonService<IFamilyDao<T>>(familyDao);
			sl.LoadSingletonService<ICrudDao<Family<T>>>(familyDao);
			sl.LoadSingletonService<IDao<Family<T>>>(familyDao);
		}

		private static void LoadNaturalnessModels<T>(HomeMadeServiceLocator sl) where T : Animal
		{
			sl.LoadDelegatedService<IFamilyCrudModel<T>>(
				() =>
				new FamilyCrudModel<T>(sl.Resolve<IConversationsContainerAccessor>(), sl.Resolve<IConversationFactory>(),
				                       sl.Resolve<IDaoFactory>()));
		}
	}
}