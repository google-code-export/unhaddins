using System;
using System.Reflection;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using uNhAddIns.Adapters.CommonTests.ConversationManagement;
using uNhAddIns.PostSharpAdapters.Tests.Model;
using uNhAddIns.PostSharpAdapters.Tests.Util;
using uNhAddIns.SessionEasier.Conversations;

namespace uNhAddIns.PostSharpAdapters.Tests
{
	[TestFixture]
	public class TestSuite
	{
		private static ServiceLocatorStub CreateDefaultContainer()
		{
			var conversationContainerStub = new ThreadLocalConversationContainerStub();
			ServiceLocatorStub slStub = ServiceLocatorStub.Create();
			slStub.AddInstance<IConversationContainer>(conversationContainerStub);
			slStub.AddInstance<IConversationsContainerAccessor>(new ConversationsContainerAccessorStub(slStub));
			conversationContainerStub.Reset();
			return slStub;
		}

		public ThreadLocalConversationContainerStub CurrentConversationContainer
		{
			get
			{
				return (ThreadLocalConversationContainerStub)ServiceLocator.Current.GetInstance<IConversationContainer>();
			}
		}

		//[TearDown]
		//public void ClearConversationContainer()
		//{
		//    if (CurrentConversationContainer != null)
		//    {
		//        CurrentConversationContainer.Reset();
		//    }
		//}

		[Test]
		public void ShouldSupportsDefaultEndMode()
		{
			ServiceLocatorStub sl = CreateDefaultContainer();
			bool endedCalled = false;
			var convFactory = new ConversationFactoryStub(delegate(string id)
			                                              	{
			                                              		IConversation result = new NoOpConversationStub(id);
			                                              		result.Ended += ((s, a) => endedCalled = true);
			                                              		return result;
			                                              	});
			sl.AddInstance<IConversationFactory>(convFactory);
			var presenter = new SamplePresenterWithDefaultModeEnd();

			presenter.GetProduct(Guid.NewGuid());
			endedCalled.Should().Be.True();

			Assert.That(CurrentConversationContainer.BindedConversationCount, Is.EqualTo(0),
			            "Don't unbind the conversation after end. The Adapter are changing the conversation AutoUnbindAfterEndConversation");
		}

		[Test]
		public void ShouldSupportsImplicitMethodsInclusion()
		{
			ServiceLocatorStub sl = CreateDefaultContainer();

			bool resumedCalled = false;
			bool startedCalled = false;
			var convFactory = new ConversationFactoryStub(delegate(string id)
			                                              	{
			                                              		IConversation result = new NoOpConversationStub(id);
			                                              		result.Resumed += ((s, a) => resumedCalled = true);
			                                              		result.Started += ((s, a) => startedCalled = true);
			                                              		return result;
			                                              	});
			sl.AddInstance<IConversationFactory>(convFactory);

			var conversationContainer = CurrentConversationContainer;

			var presenter = new SamplePresenter();

			presenter.GetProduct(Guid.NewGuid());
			Assert.That(startedCalled, "An implicit method inclusion don't start the conversation.");
			Assert.That(conversationContainer.BindedConversationCount, Is.EqualTo(1),
			            "Should have one active conversation because the default mode is continue.");
			resumedCalled = false;
			startedCalled = false;

			presenter.GetProduct(Guid.NewGuid());
			Assert.That(resumedCalled, "An implicit method inclusion don't resume the conversation.");
			Assert.That(conversationContainer.BindedConversationCount, Is.EqualTo(1),
			            "Should have one active conversation because the default mode is continue.");
			resumedCalled = false;

			presenter.DoSomethingNoPersistent();
			Assert.That(!resumedCalled, "An explicit method exclusion resume the conversation; shouldn't");
			Assert.That(conversationContainer.BindedConversationCount, Is.EqualTo(1),
			            "Should have one active conversation because the default mode is continue.");

			string value = presenter.PropertyOutConversation;
			Assert.That(!resumedCalled, "An explicit method exclusion resume the conversation; shouldn't");
			Assert.That(conversationContainer.BindedConversationCount, Is.EqualTo(1),
			            "Should have one active conversation because the default mode is continue.");

			value = presenter.PropertyInConversation;
			Assert.That(resumedCalled, "An implicit method inclusion don't resume the conversation.");
			Assert.That(conversationContainer.BindedConversationCount, Is.EqualTo(1),
			            "Should have one active conversation because the default mode is continue.");
			resumedCalled = false;

			presenter.AcceptAll();
			Assert.That(resumedCalled, "An explicit method inclusion should resume the conversation");
			Assert.That(conversationContainer.BindedConversationCount, Is.EqualTo(0),
			            "Should have NO active conversation because the method AcceptAll end the conversation.");
		}

		[Test]
		public void ShouldUnbindOnEndException()
		{
			ServiceLocatorStub sl = CreateDefaultContainer();

			var convFactory = new ConversationFactoryStub(delegate(string id)
			                                              	{
			                                              		IConversation result = new ExceptionOnFlushConversationStub(id);
			                                              		result.OnException += ((sender, args) => args.ReThrow = false);
			                                              		return result;
			                                              	});

			sl.AddInstance<IConversationFactory>(convFactory);

			var presenter = new SamplePresenter();

			presenter.GetProduct(Guid.NewGuid());
			Assert.That(CurrentConversationContainer.BindedConversationCount, Is.EqualTo(1),
			            "One conversation should be binded.");

			presenter.AcceptAll();
			Assert.That(CurrentConversationContainer.BindedConversationCount, Is.EqualTo(0),
			            "Don't unbind the conversation with exception catch by custom event handler");
		}

		[Test]
		public void ShouldUnbindOnFlushException()
		{
			var convFactory = new ConversationFactoryStub(
				id =>
					{
						IConversation result = new ExceptionOnFlushConversationStub(id);
						result.OnException += ((sender, args) => args.ReThrow = false);
						return result;
					});
			ServiceLocatorStub sl = CreateDefaultContainer();
			sl.AddInstance<IConversationFactory>(convFactory);

			var presenter = new SamplePresenter();

			Product product = presenter.GetProduct(Guid.NewGuid());

			Assert.That(CurrentConversationContainer.BindedConversationCount, Is.EqualTo(1),
			            "One conversation should be binded.");

			presenter.DeleteInmediatly(product);
			Assert.That(CurrentConversationContainer.BindedConversationCount, Is.EqualTo(0),
			            "Don't unbind the conversation with exception catch by custom event handler");
		}

		[Test]
		public void ShouldWorkWithConcreteCtorInterceptor()
		{
			ServiceLocatorStub sl = CreateDefaultContainer();
			var logger = new MyLogger();

			var convFactory = new ConversationFactoryStub(delegate(string id)
			                                              	{
			                                              		IConversation result = new NoOpConversationStub(id);
			                                              		return result;
			                                              	});
			sl.AddInstance<IConversationFactory>(convFactory);

			sl.AddInstance(logger);

			var presenter = new SamplePresenterWithConcreteInterception();

			presenter.GetProduct(Guid.NewGuid());

			logger.Messages
				.Should().Have
				.SameSequenceAs(new[]
				                	{
				                		LogInterceptor.ConversationStarting,
				                		LogInterceptor.ConversationStarted
				                	});
		}

		[Test]
		public void ShouldWorkWithConventionCtorInterceptor()
		{
			ServiceLocatorStub sl = CreateDefaultContainer();
			var logger = new MyLogger();

			var convFactory = new ConversationFactoryStub(delegate(string id)
			                                              	{
			                                              		IConversation result = new NoOpConversationStub(id);
			                                              		return result;
			                                              	});

			sl.AddInstance(logger);
			sl.AddInstance<IConversationFactory>(convFactory);
			sl.AddService<IConversationCreationInterceptorConvention<SamplePresenter>,
				LogInterceptorConvention<SamplePresenter>>();

			var presenter = new SamplePresenter();
			presenter.GetProduct(Guid.NewGuid());
			logger.Messages
				.Should()
				.Have.SameSequenceAs(new[]
				                     	{
				                     		LogInterceptor.ConversationStarting,
				                     		LogInterceptor.ConversationStarted
				                     	});
		}

		[Test]
		public void ShouldWorkWithServiceCtorInterceptor()
		{
			ServiceLocatorStub sl = CreateDefaultContainer();
			var logger = new MyLogger();

			var convFactory = new ConversationFactoryStub(delegate(string id)
			                                              	{
			                                              		IConversation result = new NoOpConversationStub(id);
			                                              		return result;
			                                              	});

			sl.AddInstance<IConversationFactory>(convFactory);
			sl.AddService<ILogInterceptor, LogInterceptor>();
			sl.AddInstance(logger);

			var presenter = new SamplePresenterWithServiceInterception();

			presenter.GetProduct(Guid.NewGuid());

			logger.Messages
				.Should().Have
				.SameSequenceAs(new[]
				                	{
				                		LogInterceptor.ConversationStarting,
				                		LogInterceptor.ConversationStarted
				                	});
		}

		[Test]
		public void ShouldSupportsExplicitMethodsInclusionOfPrivateMethod()
		{
			var sl = CreateDefaultContainer();

			var startedCalled = false;
			var convFactory = new ConversationFactoryStub(delegate(string id)
			{
			    IConversation result = new NoOpConversationStub(id);
			    result.Started += ((s, a) => startedCalled = true);
			    return result;
			});

			sl.AddInstance<IConversationFactory>(convFactory);

			
			var scm = new SamplePresenter();

			scm.GetType().GetMethod("GetProductInternal", BindingFlags.Instance | BindingFlags.NonPublic)
			    .Invoke(scm, null);

			Assert.That(startedCalled, "An explicit method inclusion of a private member don't start the conversation.");

			Assert.That(CurrentConversationContainer.BindedConversationCount, Is.EqualTo(1),
			            "Should have one active conversation because the default mode is continue.");
		}

		[Test]
		public void PrivateMethodsShouldNotBeImplicitlyIncluded()
		{
			var sl = CreateDefaultContainer();

			int startedCalledTimes = 0;
			var convFactory = new ConversationFactoryStub(delegate(string id)
			{
			    IConversation result = new NoOpConversationStub(id);
			    result.Started += (s, a) => startedCalledTimes++;
			    return result;
			});
			sl.AddInstance<IConversationFactory>(convFactory);
            
			var presenter = new SamplePresenter();

			//DoSomethingNoPersistent() calls to a private method that is not explicit included 
			presenter.DoSomethingNoPersistent();

			Assert.That(startedCalledTimes, Is.EqualTo(0),
						"The conversation should not be started.");

		}

		[Test]
		public void TwoInstancesShouldStartDifferentsConversations()
		{

			var sl = CreateDefaultContainer();

			int startedCalledTimes = 0;
			var convFactory = new ConversationFactoryStub(delegate(string id)
			{
			    IConversation result = new NoOpConversationStub(id);
			    result.Started += (s, a) => startedCalledTimes++;
			    return result;
			});
			sl.AddInstance<IConversationFactory>(convFactory);

			var presenter = new SamplePresenter();
			presenter.GetProduct(Guid.NewGuid());
			presenter.GetProduct(Guid.NewGuid());

			var presenter2 = new SamplePresenter();
			presenter2.GetProduct(Guid.NewGuid());

			startedCalledTimes.Should().Be.EqualTo(2);
		}

		[Test]
		public void ShouldSupportNestedConversationalMethods()
		{

			var sl = CreateDefaultContainer();

			int startedCalledTimes = 0;
			int resumedTimes = 0;
			int pausedTimes = 0;
			var convFactory = new ConversationFactoryStub(delegate(string id)
			{
			    IConversation result = new NoOpConversationStub(id);
			    result.Started += (s, a) => startedCalledTimes++;
			    result.Resumed += (s, a) => resumedTimes++;
			    result.Paused += (s, a) => pausedTimes++;
			    return result;
			});

			sl.AddInstance<IConversationFactory>(convFactory);

			var conversationContainer = CurrentConversationContainer;

			var presenter = new SamplePresenter();

			presenter.GetProductWithNestedMethod();

			Assert.That(startedCalledTimes, Is.EqualTo(1),
			            "The conversation should be started once in the former conversational method.");

			Assert.That(resumedTimes, Is.EqualTo(0),
			            "The conversation should not be resumed because the second conversational method is nested.");

			Assert.That(pausedTimes, Is.EqualTo(1),
			            "The conversation should be paused once because the second conversational method is nested.");

			Assert.That(conversationContainer.BindedConversationCount, Is.EqualTo(1),
			            "Should have one active conversation because the default mode is continue.");

		}


		[Test]
		public void ShouldWorkWithNoopMarker()
		{
			var sl = CreateDefaultContainer();
			sl.AddInstance(new NoopConversationalMarker {Noop = true});
			
			var presenter = new SamplePresenter();
			Assert.DoesNotThrow(() => presenter.GetProduct(Guid.NewGuid()));

		}

		[Test]
		[Description(@"A bad boy container, is a container who can resolve 
						an instance that was never registered before.
						Ninject and Unity are bad boys container.
						Windsor is a good boy.")]
		public void UsingABadBoyContainerWithoutConversationFactoryShouldThrow()
		{
			var sl = CreateDefaultContainer();
			sl.AddInstance(new NoopConversationalMarker());
			var presenter = new SamplePresenter();
			Assert.Throws<ActivationException>(() => presenter.GetProduct(Guid.NewGuid()));
		}
             

		[Test]
		public void ConversationalAttributeShouldBePersisted()
		{
			typeof(SamplePresenter).IsDefined(typeof(PersistenceConversationalAttribute), true)
				.Should("PersistMetaData should be true").Be.True();
		}


	}
}