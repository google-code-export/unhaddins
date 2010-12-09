using System;
using Castle.Windsor;
using NHibernate.Properties;

namespace uNhAddIns.CastleAdapters.EnhancedBytecodeProvider
{
	public class ReflectionOptimizer : NHibernate.Bytecode.Lightweight.ReflectionOptimizer
	{
		private readonly IWindsorContainer container;

		public ReflectionOptimizer(IWindsorContainer container, Type mappedType, IGetter[] getters, ISetter[] setters)
			: base(mappedType, getters, setters)
		{
			this.container = container;
		}

		public override object CreateInstance()
		{
			if (container.Kernel.HasComponent(mappedType))
			{
				return container.Resolve(mappedType);
			}
			else
			{
				return container.Kernel.HasComponent(mappedType.FullName)
						? container.Resolve(mappedType.FullName, new{})
				       	: base.CreateInstance();
			}
		}

		protected override void ThrowExceptionForNoDefaultCtor(Type type) {}
	}
}