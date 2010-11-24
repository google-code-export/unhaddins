using System;
using NHibernate.Bytecode;
using Spring.Objects.Factory;

namespace uNhAddIns.SpringAdapters.EnhancedBytecodeProvider
{
	public class ObjectsFactory : IObjectsFactory
	{
		private readonly IListableObjectFactory listableObjectFactory;

		public ObjectsFactory(IListableObjectFactory listableObjectFactory)
		{
			this.listableObjectFactory = listableObjectFactory;
		}

		public object CreateInstance(Type type)
		{
			var namesForType = listableObjectFactory.GetObjectNamesForType(type);
			return namesForType.Length > 0 ? listableObjectFactory.GetObject(namesForType[0], type) : Activator.CreateInstance(type);
		}

		public object CreateInstance(Type type, bool nonPublic)
		{
			var namesForType = listableObjectFactory.GetObjectNamesForType(type);
			return namesForType.Length > 0 ? listableObjectFactory.GetObject(namesForType[0], type) : Activator.CreateInstance(type);
		}

		public object CreateInstance(Type type, params object[] ctorArgs)
		{
			return Activator.CreateInstance(type, ctorArgs);
		}
	}
}