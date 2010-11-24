using System;
using LinFu.DynamicProxy;
using NHibernate;
using uNhAddIns.SessionEasier;

namespace uNhAddIns.LinFuAdapters
{
	public class SessionWrapper : ISessionWrapper
	{
		private readonly ProxyFactory proxyFactory = new ProxyFactory();

		#region Implementation of ISessionWrapper

		public ISession Wrap(ISession realSession, SessionCloseDelegate closeDelegate, SessionDisposeDelegate disposeDelegate)
		{
			var wrapper = new TransactionProtectionWrapper(realSession, closeDelegate, disposeDelegate);
			var wrapped = proxyFactory.CreateProxy<ISession>(wrapper, Commons.SessionProxyInterfaces);
			return wrapped;
		}

		public ISession WrapWithAutoTransaction(ISession realSession, SessionCloseDelegate closeDelegate, SessionDisposeDelegate disposeDelegate)
		{
			throw new NotImplementedException();
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