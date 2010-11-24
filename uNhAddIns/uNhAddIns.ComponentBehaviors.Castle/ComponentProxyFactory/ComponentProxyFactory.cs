using System;
using System.Linq;
using Castle.Core.Interceptor;
using Castle.DynamicProxy;
using Castle.MicroKernel;
using uNhAddIns.ComponentBehaviors.Castle.Configuration;

namespace uNhAddIns.ComponentBehaviors.Castle.ComponentProxyFactory
{
	public class ComponentProxyFactory : IComponentProxyFactory
	{
		private static readonly ProxyGenerator proxyGenerator = new ProxyGenerator();
		private readonly IBehaviorConfigurator _behaviorConfigurator;
		private readonly IKernel _kernel;

		public ComponentProxyFactory(IKernel kernel, IBehaviorConfigurator behaviorConfigurator)
		{
			_kernel = kernel;
			_behaviorConfigurator = behaviorConfigurator;
		}

		public T GetEntity<T>()
		{
			return (T) GetEntity(typeof (T));
		}

		public object GetEntity(Type type)
		{
			var proxyInfo = _behaviorConfigurator.GetProxyInformation(type);
			Type[] additionalInterfacesToProxy = proxyInfo.AdditionalInterfaces.ToArray();
			IInterceptor[] interceptors = proxyInfo.Interceptors.Select(i => _kernel[i]).OfType<IInterceptor>().ToArray();
			if (interceptors.Length > 0)
			{
				object instance = proxyGenerator.CreateClassProxy(proxyInfo.EntityType,
																  additionalInterfacesToProxy,
																  interceptors);
				return instance;
			}
			return Activator.CreateInstance(type);
		}
	}
}