using System;
using NHibernate.Properties;
using Spring.Objects.Factory;

namespace uNhAddIns.SpringAdapters.EnhancedBytecodeProvider
{
	public class ReflectionOptimizer : NHibernate.Bytecode.Lightweight.ReflectionOptimizer
	{
		private readonly IListableObjectFactory listableObjectFactory;

		public ReflectionOptimizer(IListableObjectFactory listableObjectFactory, Type mappedType, IGetter[] getters, ISetter[] setters) : base(mappedType, getters, setters)
		{
			this.listableObjectFactory = listableObjectFactory;
		}

		public override object CreateInstance()
		{
			var namesForType = listableObjectFactory.GetObjectNamesForType(mappedType);
			if (namesForType.Length > 0)
			{
				return listableObjectFactory.GetObject(namesForType[0], mappedType);
			}
			else
			{
				return base.CreateInstance();
			}
		}

		protected override void ThrowExceptionForNoDefaultCtor(Type type) { }
	}
}