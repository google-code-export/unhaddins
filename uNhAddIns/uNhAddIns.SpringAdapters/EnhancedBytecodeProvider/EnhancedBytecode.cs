using System;
using NHibernate.Bytecode;
using NHibernate.ByteCode.Spring;
using NHibernate.Properties;
using NHibernate.Type;
using Spring.Objects.Factory;

namespace uNhAddIns.SpringAdapters.EnhancedBytecodeProvider
{
	public class EnhancedBytecode : IBytecodeProvider
	{
		// A better place for this classes is Spring.Data.NHibernate
		private readonly IListableObjectFactory listableObjectFactory;
		private readonly IObjectsFactory objectsFactory;
		private readonly DefaultCollectionTypeFactory collectionTypefactory;

		public EnhancedBytecode(IListableObjectFactory listableObjectFactory)
		{
			this.listableObjectFactory = listableObjectFactory;
			objectsFactory = new ObjectsFactory(listableObjectFactory);
			collectionTypefactory = new DefaultCollectionTypeFactory();
		}

		public IReflectionOptimizer GetReflectionOptimizer(Type clazz, IGetter[] getters, ISetter[] setters)
		{
			return new ReflectionOptimizer(listableObjectFactory, clazz, getters, setters);
		}

		public IProxyFactoryFactory ProxyFactoryFactory
		{
			get { return new ProxyFactoryFactory(); }
		}

		public IObjectsFactory ObjectsFactory
		{
			get { return objectsFactory; }
		}

		public ICollectionTypeFactory CollectionTypeFactory
		{
			get { return collectionTypefactory; }
		}
	}
}