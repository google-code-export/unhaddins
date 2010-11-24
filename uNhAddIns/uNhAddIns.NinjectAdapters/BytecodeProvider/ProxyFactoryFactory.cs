using System;
using Ninject;
using NHibernate.Bytecode;

namespace uNhAddIns.NinjectAdapters.BytecodeProvider
{
    public class ProxyFactoryFactory : IProxyFactoryFactory 
    {

        public ProxyFactoryFactory(IKernel Kernel)
        {
            kernel = Kernel;
        }

        private readonly IKernel kernel;


        #region IProxyFactoryFactory Members

        NHibernate.Proxy.IProxyFactory IProxyFactoryFactory.BuildProxyFactory()
        {
            return kernel.Get<NHibernate.Proxy.IProxyFactory>();
        }

    	public bool IsInstrumented(Type entityClass)
    	{
    		return false;
    	}

    	NHibernate.Proxy.IProxyValidator IProxyFactoryFactory.ProxyValidator
        {
            get {return new Proxy.ProxyTypeValidator();}
        }

        #endregion
    }
}
