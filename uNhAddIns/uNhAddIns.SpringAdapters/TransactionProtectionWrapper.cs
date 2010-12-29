using System;
using System.Reflection;
using AopAlliance.Intercept;
using NHibernate;
using NHibernate.Util;
using Spring.Reflection.Dynamic;
using uNhAddIns.SessionEasier;
using Spring.Aop;

namespace uNhAddIns.SpringAdapters
{
	[Serializable]
	public class TransactionProtectionWrapper : BasicTransactionProtectionWrapper, IMethodInterceptor, ITargetSource
	{
		public TransactionProtectionWrapper(ISession realSession, SessionCloseDelegate closeDelegate)
			: base(realSession, closeDelegate) { }

		public TransactionProtectionWrapper(ISession realSession, SessionCloseDelegate closeDelegate,
																				SessionDisposeDelegate disposeDelegate)
			: base(realSession, closeDelegate, disposeDelegate) { }

		#region Implementation of IMethodInterceptor

		public object Invoke(IMethodInvocation invocation)
		{
			try
			{
				MethodInfo methodInfo = invocation.Method;
				object returnValue = base.Invoke(methodInfo, invocation.Arguments);

				if (returnValue != InvokeImplementation)
				{
					return returnValue;
				}

				var method = new SafeMethod(methodInfo);
				return method.Invoke(realSession, invocation.Arguments);
			}
			catch (TargetInvocationException ex)
			{
				throw ReflectHelper.UnwrapTargetInvocationException(ex);
			}
		}

		#endregion

		#region Implementation of ITargetSource

		public object GetTarget()
		{
			return realSession;
		}

		public void ReleaseTarget(object target)
		{
			
		}

		public Type TargetType
		{
			get { return typeof (ISession); }
		}

		public bool IsStatic
		{
			get { return false; }
		}

		#endregion
	}
}