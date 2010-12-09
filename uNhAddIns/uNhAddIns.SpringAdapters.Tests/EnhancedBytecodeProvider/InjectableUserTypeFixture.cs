using NHibernate.Bytecode;
using NUnit.Framework;
using Spring.Context;
using Spring.Context.Support;
using Spring.Objects.Factory.Config;
using uNhAddIns.Adapters.CommonTests.EnhancedBytecodeProvider;
using uNhAddIns.SpringAdapters.EnhancedBytecodeProvider;

namespace uNhAddIns.SpringAdapters.Tests.EnhancedBytecodeProvider
{
	[TestFixture]
	public class InjectableUserTypeFixture : AbstractInjectableUserTypeFixture
	{
		IConfigurableListableObjectFactory objectFactory;

		protected override void InitializeServiceLocator()
		{
			IConfigurableApplicationContext context = new StaticApplicationContext();
			objectFactory = context.ObjectFactory;

			objectFactory.Register<IDelimiter, ParenDelimiter>();
			objectFactory.RegisterPrototype<InjectableStringUserType>(); 
		}

		protected override IBytecodeProvider GetBytecodeProvider()
		{
			return new EnhancedBytecode(objectFactory);
		}
	}
}