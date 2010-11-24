using System;
using NHibernate;
using uNhAddIns.SessionEasier;

namespace uNhAddIns.SpringAdapters
{
	public class SessionWrapper : ISessionWrapper
	{
		[Serializable]
		private class SerializableProxyFactory : Spring.Aop.Framework.ProxyFactory
		{
			// ensure proxy types are generated as Serializable
			public override bool IsSerializable
			{
				get { return true; }
			}
		}

		#region Implementation of ISessionWrapper

		public ISession Wrap(ISession realSession, SessionCloseDelegate closeDelegate, SessionDisposeDelegate disposeDelegate)
		{
			if (IsWrapped(realSession))
			{
				return realSession;
			}
			var wrapper = new TransactionProtectionWrapper(realSession, closeDelegate, disposeDelegate);

			return BuildSessionWrapped(wrapper);
		}

		private static ISession BuildSessionWrapped(TransactionProtectionWrapper wrapper)
		{
			var proxyFactory = new SerializableProxyFactory
			                   	{
			                   		Interfaces = Commons.SessionProxyInterfaces,
			                   		TargetSource = wrapper,
			                   		ProxyTargetType = false
			                   	};
			proxyFactory.AddAdvice(wrapper);

			return (ISession) proxyFactory.GetProxy();
		}

		public ISession WrapWithAutoTransaction(ISession realSession, SessionCloseDelegate closeDelegate, SessionDisposeDelegate disposeDelegate)
		{
			if (IsWrapped(realSession))
			{
				return realSession;
			}
			var wrapper = new AutoTransactionProtectionWrapper(realSession, closeDelegate, disposeDelegate);

			return BuildSessionWrapped(wrapper);
		}

		public bool IsWrapped(ISession session)
		{
			if (session == null)
			{
				return false;
			}
			var sessionProxy = session as ISessionProxy;
			// try to make sure we don't wrap and already wrapped session
			return sessionProxy != null && sessionProxy.InvocationHandler != null
						 && sessionProxy.InvocationHandler is TransactionProtectionWrapper;
		}

		#endregion
	}
}