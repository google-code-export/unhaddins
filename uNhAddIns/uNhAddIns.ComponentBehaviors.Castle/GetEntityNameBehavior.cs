using System;
using System.Linq;
using Castle.DynamicProxy;
using uNhAddIns.NHibernateTypeResolver;

namespace uNhAddIns.ComponentBehaviors.Castle
{
	[Behavior(1, typeof (IWellKnownProxy))]
	public class GetEntityNameBehavior : IInterceptor
	{
		#region IInterceptor Members

		public void Intercept(IInvocation invocation)
		{
			if (invocation.Method.DeclaringType.Equals(typeof (IWellKnownProxy)))
			{
				Type baseType = GetBaseType(invocation);

				if (invocation.Method.Name == "get_EntityName")
				{
					invocation.ReturnValue = baseType.FullName;
				}
				else if (invocation.Method.Name == "get_EntityType")
				{
					invocation.ReturnValue = baseType;
				}
			}
			else
			{
				invocation.Proceed();
			}
		}

		#endregion

		public Type GetBaseType(IInvocation invocation)
		{
			object target = ((IProxyTargetAccessor) invocation.Proxy).DynProxyGetTarget();
			if (ReferenceEquals(invocation.Proxy, target))
			{
				return invocation.Proxy.GetType().BaseType;
			}
			return invocation.Proxy.GetType().GetInterfaces().First();
		}
	}
}