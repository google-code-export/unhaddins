using System;
using System.Reflection;
using AopAlliance.Intercept;
using Spring.Objects.Factory;
using uNhAddIns.Adapters.Common;
using uNhAddIns.SessionEasier.Conversations;

namespace uNhAddIns.SpringAdapters.ConversationManagement
{
	public class ConversationInterceptor : AbstractConversationInterceptor, IMethodInterceptor, IObjectFactoryAware
	{
		private IObjectFactory factory;

		public ConversationInterceptor(IConversationalMetaInfoStore metadataStore,
		                               IConversationsContainerAccessor conversationsContainerAccessor,
		                               IConversationFactory conversationFactory)
			: base(metadataStore, conversationsContainerAccessor, conversationFactory) {}

		public Type TargetType { get; set; }

		#region IMethodInterceptor Members

		public object Invoke(IMethodInvocation invocation)
		{
			TargetType = invocation.TargetType;
			MethodInfo methodInfo = invocation.Method;
			if (!ShouldBeIntercepted(methodInfo))
			{
				return invocation.Proceed();
			}
			else
			{
				BeforeMethodExecution(methodInfo);
				try
				{
					object result = invocation.Proceed();
					AfterMethodExecution(methodInfo);
					return result;
				}
				catch (Exception)
				{
					DisposeConversationOnException();
					throw;
				}
			}
		}

		#endregion

		#region IObjectFactoryAware Members

		public IObjectFactory ObjectFactory
		{
			set { factory = value; }
		}

		#endregion

		protected override Type GetConversationalImplementor()
		{
			return TargetType;
		}

		protected override IConversationCreationInterceptor GetConversationCreationInterceptor(Type configuredConcreteType)
		{
			// TODO: Spring throws an exception when the type does not exist so, for the moment,
			// this is a restriction to register the IConversationCreationInterceptor
			// The restriction : IConversationCreationInterceptor, if registered, should be registered
			// with its fullname as key.
			if (factory.ContainsObject(configuredConcreteType.FullName))
			{
				return (IConversationCreationInterceptor) factory.GetObject(configuredConcreteType.FullName, configuredConcreteType);
			}
			return null;
		}
	}
}