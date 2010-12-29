using System;
using LinFu.DynamicProxy;
using NHibernate;
using NHibernate.Engine;
using NUnit.Framework;
using uNhAddIns.SessionEasier.Conversations;
using IInterceptor=LinFu.DynamicProxy.IInterceptor;
using uNhAddIns.SessionEasier.Contexts;

namespace uNhAddIns.Test.Conversations
{
	[TestFixture]
	public class NhConversationsContainerAccessorFixture
	{
		private static readonly ProxyFactory proxyGenerator = new ProxyFactory();

		private class InvalidSessionFactory : IInterceptor
		{
			#region IInterceptor Members

			public object Intercept(InvocationInfo info)
			{
				return null;
			}

			#endregion
		}

		private class SessionFactoryMock : IInterceptor
		{
			public object SessionContext { get; set; }

			#region IInterceptor Members

			public object Intercept(InvocationInfo info)
			{
				return "get_CurrentSessionContext".Equals(info.TargetMethod.Name) ? SessionContext : null;
			}

			#endregion
		}

		[Test]
		public void Ctor()
		{
			Assert.Throws<ArgumentNullException>(() => new NhConversationsContainerAccessor(null));
			Assert.Throws<ConversationException>(() => new NhConversationsContainerAccessor(new ISessionFactory[0]));
			var invalidSFmock = proxyGenerator.CreateProxy<ISessionFactory>(new InvalidSessionFactory());
			Assert.Throws<ConversationException>(() => new NhConversationsContainerAccessor(new[] { invalidSFmock }));

			var notvalidSFmock = proxyGenerator.CreateProxy<ISessionFactoryImplementor>(new SessionFactoryMock(),
																																			 new[]
			                                                                 	{
			                                                                 		typeof (ISessionFactory)
			                                                                 	});
			Assert.Throws<ConversationException>(() => new NhConversationsContainerAccessor(new[] { notvalidSFmock }));

			var container = new ThreadLocalConversationalSessionContext(null);
			var validSFmock = proxyGenerator.CreateProxy<ISessionFactoryImplementor>(new SessionFactoryMock { SessionContext = container },
																																		new[]
			                                                              	{
			                                                              		typeof (ISessionFactory)
			                                                              	});
			var ca = new NhConversationsContainerAccessor(new[] { validSFmock });
			Assert.That(ca.Container, Is.SameAs(container));

			var sessionFactoryMock = new SessionFactoryMock();
			var contexMock = proxyGenerator.CreateProxy<ISessionFactoryImplementor>(sessionFactoryMock,
			                                                                        new[] {typeof (ISessionFactory)});
			sessionFactoryMock.SessionContext = new ThreadLocalSessionContext(contexMock);
			try
			{
				new NhConversationsContainerAccessor(new[] { contexMock });
			}
			catch (ConversationException e)
			{
				Assert.That(e.Message, Text.Contains("Current session context does not implement IConversationContainer"));
			}
		}
	}
}