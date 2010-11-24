using System;
using Castle.MicroKernel;
using NHibernate.Bytecode;
using NHibernate.Proxy;
using uNhAddIns.ComponentBehaviors.Castle.Configuration;
using IProxyFactory=NHibernate.Proxy.IProxyFactory;

namespace uNhAddIns.ComponentBehaviors.Castle.ProxyFactory
{
    public class ComponentProxyFactoryFactory : IProxyFactoryFactory
    {
        private readonly IBehaviorConfigurator _behaviorConfigurator;
    	readonly IKernel _kernel;

    	public ComponentProxyFactoryFactory(IBehaviorConfigurator behaviorConfigurator, IKernel kernel)
    	{
    		_behaviorConfigurator = behaviorConfigurator;
    		_kernel = kernel;
    	}

    	#region IProxyFactoryFactory Members

        /// <summary>
        /// Build a proxy factory specifically for handling runtime
        ///             lazy loading. 
        /// </summary>
        /// <returns>
        /// The lazy-load proxy factory. 
        /// </returns>
        public IProxyFactory BuildProxyFactory()
        {
			return new ComponentProxyFactory(_behaviorConfigurator, _kernel);
        }

    	public bool IsInstrumented(Type entityClass)
    	{
    		return false;
    	}

    	public IProxyValidator ProxyValidator
        {
            get { return new DynProxyTypeValidator(); }
        }

        #endregion
    }
}