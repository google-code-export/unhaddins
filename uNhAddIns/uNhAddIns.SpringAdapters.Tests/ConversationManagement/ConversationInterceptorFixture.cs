using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using Spring.Context;
using Spring.Context.Support;
using Spring.Objects.Factory.Config;
using uNhAddIns.Adapters.CommonTests;
using uNhAddIns.Adapters.CommonTests.ConversationManagement;
using uNhAddIns.SessionEasier.Conversations;

namespace uNhAddIns.SpringAdapters.Tests.ConversationManagement
{
	public interface ISpringConfigurationAccessor
	{
		IConfigurableListableObjectFactory ObjectFactory { get; }
	}

	[TestFixture]
	public class ConversationInterceptorFixture : ConversationFixtureBase
	{
		private class TestSpringConfigurationAccessor : ISpringConfigurationAccessor
		{
			private readonly IConfigurableListableObjectFactory objectFactory;

			public TestSpringConfigurationAccessor(IConfigurableListableObjectFactory objectFactory)
			{
				this.objectFactory = objectFactory;
			}

			#region Implementation of IContainerAccessor

			public IConfigurableListableObjectFactory ObjectFactory
			{
				get { return objectFactory; }
			}

			#endregion
		}

		protected override IServiceLocator NewServiceLocator()
		{
			IConfigurableApplicationContext context = new StaticApplicationContext();
			var objectFactory = context.ObjectFactory;
			objectFactory.RegisterInstance<ISpringConfigurationAccessor>(new TestSpringConfigurationAccessor(objectFactory));
			objectFactory.RegisterDefaultConversationAop();
			// Services for this test
			var sl = new SpringServiceLocatorAdapter(objectFactory);
			objectFactory.RegisterInstance<IServiceLocator>(sl);

			objectFactory.Register<IConversationContainer, ThreadLocalConversationContainerStub>();
			objectFactory.Register<IConversationsContainerAccessor,ConversationsContainerAccessorStub>();
			objectFactory.Register<IDaoFactory, DaoFactoryStub>();
			objectFactory.Register<ISillyDao, SillyDaoStub>();

			return sl;
		}

		protected override void RegisterAsTransient<TService, TImplementor>(IServiceLocator serviceLocator)
		{
			var ca = serviceLocator.GetInstance<ISpringConfigurationAccessor>();
			ca.ObjectFactory.RegisterPrototype<TService, TImplementor>();
		}

		protected override void RegisterInstanceForService<T>(IServiceLocator serviceLocator, T instance)
		{
			var ca = serviceLocator.GetInstance<ISpringConfigurationAccessor>();
			ca.ObjectFactory.RegisterInstance(instance);
		}
	}
}