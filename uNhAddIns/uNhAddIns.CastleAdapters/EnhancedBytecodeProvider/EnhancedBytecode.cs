using System;
using Castle.Windsor;
using NHibernate.Bytecode;
using NHibernate.ByteCode.Castle;
using NHibernate.Properties;
using NHibernate.Type;

namespace uNhAddIns.CastleAdapters.EnhancedBytecodeProvider
{
    public class EnhancedBytecode : IBytecodeProvider, IInjectableCollectionTypeFactoryClass,
                                    IInjectableProxyFactoryFactory
    {
        private readonly IWindsorContainer container;
        private readonly IObjectsFactory objectsFactory;

        private ICollectionTypeFactory collectionTypeFactory;
        private IProxyFactoryFactory proxyFactoryFactory;
    	private Type proxyFactoryFactoryType = typeof (ProxyFactoryFactory);
    	private Type colletionTypeFactoryType = typeof (DefaultCollectionTypeFactory);

        public EnhancedBytecode(IWindsorContainer container)
        {
            this.container = container;
            objectsFactory = new ObjectsFactory(container);
        }

        #region IBytecodeProvider Members

        public IReflectionOptimizer GetReflectionOptimizer(Type clazz, IGetter[] getters, ISetter[] setters)
        {
            return new ReflectionOptimizer(container, clazz, getters, setters);
        }

        public IProxyFactoryFactory ProxyFactoryFactory
        {
            get
            {
				if(proxyFactoryFactory == null)
				{
					if (container.Kernel.HasComponent(proxyFactoryFactoryType))
						proxyFactoryFactory = (IProxyFactoryFactory)container.Resolve(proxyFactoryFactoryType);
					else
						proxyFactoryFactory = (IProxyFactoryFactory)Activator.CreateInstance(proxyFactoryFactoryType);	
				}
            	return proxyFactoryFactory;
            }
        }

        public IObjectsFactory ObjectsFactory
        {
            get { return objectsFactory; }
        }

        public ICollectionTypeFactory CollectionTypeFactory
        {
            get
            {
				if(collectionTypeFactory == null)
				{
					if (container.Kernel.HasComponent(colletionTypeFactoryType))
						collectionTypeFactory = (ICollectionTypeFactory)container.Resolve(colletionTypeFactoryType);
					else
						collectionTypeFactory = (ICollectionTypeFactory)Activator.CreateInstance(colletionTypeFactoryType);
				}
				return collectionTypeFactory;
            }
        }

        #endregion

        #region IInjectableCollectionTypeFactoryClass Members

        public void SetCollectionTypeFactoryClass(string typeAssemblyQualifiedName)
        {
            SetCollectionTypeFactoryClass(Type.GetType(typeAssemblyQualifiedName, true));
        }

        public void SetCollectionTypeFactoryClass(Type type)
        {
        	colletionTypeFactoryType = type;
        }

        #endregion

        #region IInjectableProxyFactoryFactory Members

        public void SetProxyFactoryFactory(string typeName)
        {
            proxyFactoryFactoryType = Type.GetType(typeName, true);
        }

        #endregion

		
    }
}