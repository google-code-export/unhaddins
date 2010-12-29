using System;
using System.Reflection;
using LinFu.DynamicProxy;
using NHibernate;
using NHibernate.Util;
using uNhAddIns.SessionEasier;
using IInterceptor=LinFu.DynamicProxy.IInterceptor;

namespace uNhAddIns.LinFuAdapters
{
	[Serializable]
	public class TransactionProtectionWrapper : BasicTransactionProtectionWrapper, IInterceptor
	{
		#region Implementation of IInterceptor

		public TransactionProtectionWrapper(ISession realSession, SessionCloseDelegate closeDelegate)
			: base(realSession, closeDelegate) {}

		public TransactionProtectionWrapper(ISession realSession, SessionCloseDelegate closeDelegate,
		                                    SessionDisposeDelegate disposeDelegate)
			: base(realSession, closeDelegate, disposeDelegate) {}

		public object Intercept(InvocationInfo info)
		{
			try
			{
				object returnValue = base.Invoke(info.TargetMethod, info.Arguments);

				// Avoid invoking the actual implementation
				return returnValue != InvokeImplementation ? returnValue : info.TargetMethod.Invoke(realSession, info.Arguments);
			}
			catch (TargetInvocationException ex)
			{
				throw ReflectHelper.UnwrapTargetInvocationException(ex);
			}
		}

		#endregion
	}
}