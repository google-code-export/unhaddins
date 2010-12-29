using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;
using uNhAddIns.SessionEasier;
using uNhAddIns.SessionEasier.Conversations;
using uNhAddIns.SpringAdapters.ConversationManagement;

namespace uNhAddIns.SpringAdapters
{
	public static class SpringRegistrationExtensions
	{
		private static readonly IObjectDefinitionFactory ObjectDefinitionFactory = new DefaultObjectDefinitionFactory();

		public static void RegisterDefaultConversationAop(this IConfigurableListableObjectFactory confObjFactory)
		{
			var metaInfoStore = new ReflectionConversationalMetaInfoSource();
			confObjFactory.RegisterInstance(metaInfoStore);
			// register advisor definition
			var pc =
				ObjectDefinitionBuilder.RootObjectDefinition(ObjectDefinitionFactory,
				                                             typeof (ConversationInterceptor))
																										 .SetAutowireMode(AutoWiringMode.AutoDetect)
																										 .SetSingleton(false);

			confObjFactory.RegisterObjectDefinition("PersistentConversationalInterceptor", pc.ObjectDefinition);
			var postProcessor = new ConversationalAttributeAutoProxyCreator(metaInfoStore)
			          	{
										ObjectFactory = confObjFactory, 
										InterceptorNames = new[] {"PersistentConversationalInterceptor"}
									};

			confObjFactory.AddObjectPostProcessor(postProcessor);
		}

		public static void Register<TSerivice, TImplementation>(this IConfigurableListableObjectFactory confObjFactory)
		{
			var odb = ObjectDefinitionBuilder.RootObjectDefinition(ObjectDefinitionFactory, typeof(TImplementation)).SetAutowireMode(AutoWiringMode.Constructor);
			confObjFactory.RegisterObjectDefinition(typeof(TSerivice).FullName, odb.ObjectDefinition);
		}

		public static void RegisterPrototype<TSerivice, TImplementation>(this IConfigurableListableObjectFactory confObjFactory)
		{
			var odb = ObjectDefinitionBuilder.RootObjectDefinition(ObjectDefinitionFactory, typeof(TImplementation))
				.SetSingleton(false).SetAutowireMode(AutoWiringMode.AutoDetect);
			confObjFactory.RegisterObjectDefinition(typeof(TSerivice).FullName, odb.ObjectDefinition);
		}

		public static void RegisterPrototype<TImplementation>(this IConfigurableListableObjectFactory confObjFactory)
		{
			var odb = ObjectDefinitionBuilder.RootObjectDefinition(ObjectDefinitionFactory, typeof(TImplementation))
				.SetSingleton(false).SetAutowireMode(AutoWiringMode.AutoDetect);
			confObjFactory.RegisterObjectDefinition(typeof(TImplementation).FullName, odb.ObjectDefinition);
		}

		public static void RegisterInstance<TSerivice>(this IConfigurableListableObjectFactory confObjFactory, TSerivice instance)
		{
			confObjFactory.RegisterSingleton(typeof(TSerivice).FullName, instance);
		}

		public static void RegisterDefaultPersistentConversationServices(this IConfigurableListableObjectFactory confObjFactory)
		{
			confObjFactory.Register<ISessionFactoryProvider, SessionFactoryProvider>();
			confObjFactory.Register<ISessionWrapper, SessionWrapper>();
			confObjFactory.Register<IConversationFactory, DefaultConversationFactory>();
			confObjFactory.Register<IConversationsContainerAccessor, NhConversationsContainerAccessor>();
		}
	}
}