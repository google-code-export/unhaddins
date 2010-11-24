using System;
using System.Linq;
using Castle.DynamicProxy;
using Castle.MicroKernel;
using log4net;
using NHibernate;
using NHibernate.ByteCode.Castle;
using NHibernate.Engine;
using NHibernate.Proxy;
using uNhAddIns.ComponentBehaviors.Castle.Configuration;
using IInterceptor = Castle.DynamicProxy.IInterceptor;

namespace uNhAddIns.ComponentBehaviors.Castle.ProxyFactory
{
	public class ComponentProxyFactory : AbstractProxyFactory
	{
		// Fields
		protected static readonly ILog log = LogManager.GetLogger(typeof (ComponentProxyFactory));
		static readonly ProxyGenerator ProxyGenerator = new ProxyGenerator();
		readonly IBehaviorConfigurator _behaviorConfigurator;
		readonly IKernel _kernel;

		// Methods
		public ComponentProxyFactory(IBehaviorConfigurator behaviorConfigurator, IKernel kernel)
		{
			_behaviorConfigurator = behaviorConfigurator;
			_kernel = kernel;
		}

		// Properties
		protected static ProxyGenerator DefaultProxyGenerator
		{
			get { return ProxyGenerator; }
		}

		public override INHibernateProxy GetProxy(object id, ISessionImplementor session)
		{
			INHibernateProxy proxy;
			try
			{
				var behaviorInfo = _behaviorConfigurator.GetProxyInformation(PersistentClass);


				var initializer = new LazyInitializer(EntityName, PersistentClass, id, GetIdentifierMethod,
				                                      SetIdentifierMethod, ComponentIdType, session);

				IInterceptor[] interceptors = behaviorInfo.Interceptors
														.Select(i => _kernel.Resolve(i))
														.OfType<IInterceptor>()
                                                        .Union(new[] {initializer}).ToArray();

				Type[] interfaces = Interfaces.Union(behaviorInfo.AdditionalInterfaces).ToArray();

				object obj2 = IsClassProxy
				              	? ProxyGenerator.CreateClassProxy(PersistentClass, interfaces, interceptors)
                                : ProxyGenerator.CreateInterfaceProxyWithoutTarget(interfaces[0], interfaces,
				              	                                                   new IInterceptor[] {initializer});
				initializer._constructed = true;
				proxy = (INHibernateProxy) obj2;
			}
			catch (Exception exception)
			{
				log.Error("Creating a proxy instance failed", exception);
				throw new HibernateException("Creating a proxy instance failed", exception);
			}
			return proxy;
		}
	}
}