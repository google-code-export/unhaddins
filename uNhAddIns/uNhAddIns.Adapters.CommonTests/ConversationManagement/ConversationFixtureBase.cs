using log4net.Config;
using log4net.Core;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using NUnit.Framework.Syntax.CSharp;
using uNhAddIns.SessionEasier.Conversations;

namespace uNhAddIns.Adapters.CommonTests.ConversationManagement
{
	public abstract class ConversationFixtureBase
	{
		protected ConversationFixtureBase()
		{
			XmlConfigurator.Configure();
		}

		/// <summary>
		/// Initialize a new ServiceLocator registering base services needed by this test.
		/// </summary>
		/// <remarks>
		/// Services needed, in this test, are:
		/// 
		/// - Microsoft.Practices.ServiceLocation.IServiceLocator
		///		The service locator itself is used by the implementation of DaoFactory
		/// 
		/// - uNhAddIns.SessionEasier.Conversations.IConversationContainer
		///		Implementation: uNhAddIns.Adapters.CommonTests.ConversationManagement.ThreadLocalConversationContainerStub
		/// 
		/// - uNhAddIns.SessionEasier.Conversations.IConversationsContainerAccessor
		///		Implementation: uNhAddIns.Adapters.CommonTests.ConversationManagement.ConversationsContainerAccessorStub
		/// 
		/// - uNhAddIns.Adapters.CommonTests.IDaoFactory
		///		Implementation: uNhAddIns.Adapters.CommonTests.ConversationManagement.DaoFactoryStub
		/// 
		/// - uNhAddIns.Adapters.CommonTests.ISillyDao
		///		Implementation : uNhAddIns.Adapters.CommonTests.ConversationManagement.SillyDaoStub
		/// 
		/// </remarks>
		protected abstract IServiceLocator NewServiceLocator();

		/// <summary>
		/// Register a new service and its implementor as Transient (new instance per service get)
		/// </summary>
		/// <typeparam name="TService">The service type.</typeparam>
		/// <typeparam name="TImplementor">The type of the implementor.</typeparam>
		/// <param name="serviceLocator">The ServiceLocator instance</param>
		protected abstract void RegisterAsTransient<TService, TImplementor>(IServiceLocator serviceLocator)
			where TService : class where TImplementor : TService;

		/// <summary>
		/// Enlist an instance of a service.
		/// </summary>
		/// <typeparam name="T">The type of the service.</typeparam>
		/// <param name="serviceLocator">The ServiceLocator instance</param>
		/// <param name="instance">The instance of the given service type.</param>
		protected abstract void RegisterInstanceForService<T>(IServiceLocator serviceLocator, T instance);

		[Test]
		public void ShouldUnbindOnFlushException()
		{
			IServiceLocator serviceLocator = NewServiceLocator();
			RegisterAsTransient<ISillyCrudModel, SillyCrudModel>(serviceLocator);
			var convFactory = new ConversationFactoryStub(delegate(string id)
			                                              	{
			                                              		IConversation result = new ExceptionOnFlushConversationStub(id);
			                                              		result.OnException += ((sender, args) => args.ReThrow = false);
			                                              		return result;
			                                              	});
			RegisterInstanceForService<IConversationFactory>(serviceLocator, convFactory);

			var scm = serviceLocator.GetInstance<ISillyCrudModel>();
			Silly e = scm.GetIfAvailable(1);
			var conversationContainer =
				(ThreadLocalConversationContainerStub) serviceLocator.GetInstance<IConversationContainer>();
			Assert.That(conversationContainer.BindedConversationCount, Is.EqualTo(1),
			            "Don't start and bind the conversation inmediately");
			scm.ImmediateDelete(e);
			Assert.That(conversationContainer.BindedConversationCount, Is.EqualTo(0),
			            "Don't unbind the conversation with exception catch by custom event handler");
		}

		[Test]
		public void ShouldUnbindOnEndException()
		{
			IServiceLocator serviceLocator = NewServiceLocator();

			RegisterAsTransient<ISillyCrudModel, SillyCrudModel>(serviceLocator);
			var convFactory = new ConversationFactoryStub(delegate(string id)
			                                              	{
			                                              		IConversation result = new ExceptionOnFlushConversationStub(id);
			                                              		result.OnException += ((sender, args) => args.ReThrow = false);
			                                              		return result;
			                                              	});
			RegisterInstanceForService<IConversationFactory>(serviceLocator, convFactory);

			var scm = serviceLocator.GetInstance<ISillyCrudModel>();
			scm.GetIfAvailable(1);
			var conversationContainer =
				(ThreadLocalConversationContainerStub) serviceLocator.GetInstance<IConversationContainer>();
			Assert.That(conversationContainer.BindedConversationCount, Is.EqualTo(1),
			            "Don't start and bind the conversation inmediately");
			scm.AcceptAll();
			Assert.That(conversationContainer.BindedConversationCount, Is.EqualTo(0),
			            "Don't unbind the conversation with exception catch by custom event handler");
		}

		[Test]
		public void ShouldWorkWithConcreteCtorInterceptor()
		{
			IServiceLocator serviceLocator = NewServiceLocator();

			RegisterAsTransient<ISillyCrudModel, InheritedSillyCrudModelWithConcreteConversationCreationInterceptor>(
				serviceLocator);
			var convFactory = new ConversationFactoryStub(delegate(string id)
			                                              	{
			                                              		IConversation result = new NoOpConversationStub(id);
			                                              		return result;
			                                              	});
			RegisterInstanceForService<IConversationFactory>(serviceLocator, convFactory);

			var scm = serviceLocator.GetInstance<ISillyCrudModel>();
			using (var ls = new LogSpy(typeof (ConversationCreationInterceptor)))
			{
				scm.GetIfAvailable(1);
				LoggingEvent[] msgs = ls.Appender.GetEvents();
				Assert.That(msgs.Length, Is.EqualTo(2));
				Assert.That(msgs[0].RenderedMessage, Text.Contains("Starting"));
				Assert.That(msgs[1].RenderedMessage, Text.Contains("Started"));
			}
		}

		[Test]
		public void ShouldWorkWithServiceCtorInterceptor()
		{
			IServiceLocator serviceLocator = NewServiceLocator();

			RegisterAsTransient<ISillyCrudModel, InheritedSillyCrudModelWithServiceConversationCreationInterceptor>(
				serviceLocator);
			var convFactory = new ConversationFactoryStub(delegate(string id)
			                                              	{
			                                              		IConversation result = new NoOpConversationStub(id);
			                                              		return result;
			                                              	});
			RegisterInstanceForService<IConversationFactory>(serviceLocator, convFactory);

			// Registr the IConversationCreationInterceptor implementation
			RegisterAsTransient<IMyServiceConversationCreationInterceptor, ConversationCreationInterceptor>(serviceLocator);

			var scm = serviceLocator.GetInstance<ISillyCrudModel>();
			using (var ls = new LogSpy(typeof (ConversationCreationInterceptor)))
			{
				scm.GetIfAvailable(1);
				LoggingEvent[] msgs = ls.Appender.GetEvents();
				Assert.That(msgs.Length, Is.EqualTo(2));
				Assert.That(msgs[0].RenderedMessage, Text.Contains("Starting"));
				Assert.That(msgs[1].RenderedMessage, Text.Contains("Started"));
			}
		}

		[Test]
		public void ShouldWorkWithConventionCtorInterceptor()
		{
			IServiceLocator serviceLocator = NewServiceLocator();

			RegisterAsTransient<ISillyCrudModel, InheritedSillyCrudModelWithConvetionConversationCreationInterceptor>(
				serviceLocator);
			var convFactory = new ConversationFactoryStub(delegate(string id)
			                                              	{
			                                              		IConversation result = new NoOpConversationStub(id);
			                                              		return result;
			                                              	});
			RegisterInstanceForService<IConversationFactory>(serviceLocator, convFactory);

			// Registr the IConversationCreationInterceptor implementation
			RegisterAsTransient
				<IConversationCreationInterceptorConvention<InheritedSillyCrudModelWithConvetionConversationCreationInterceptor>,
					ConvetionConversationCreationInterceptor>(serviceLocator);

			var scm = serviceLocator.GetInstance<ISillyCrudModel>();
			using (var ls = new LogSpy(typeof (ConvetionConversationCreationInterceptor)))
			{
				scm.GetIfAvailable(1);
				LoggingEvent[] msgs = ls.Appender.GetEvents();
				Assert.That(msgs.Length, Is.EqualTo(2));
				Assert.That(msgs[0].RenderedMessage, Text.Contains("Starting with convention"));
				Assert.That(msgs[1].RenderedMessage, Text.Contains("Started with convention"));
			}
		}
	}
}