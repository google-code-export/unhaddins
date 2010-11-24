using System;
using Microsoft.Practices.ServiceLocation;
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
	public class EntityInjectionFixture : AbstractEntityInjectionFixture
	{
		IConfigurableListableObjectFactory objectFactory;

		protected override void InitializeServiceLocator()
		{
			IConfigurableApplicationContext context = new StaticApplicationContext();
			objectFactory = context.ObjectFactory;
			var sl = new SpringServiceLocatorAdapter(objectFactory);
			objectFactory.RegisterInstance<IServiceLocator>(sl);
			ServiceLocator.SetLocatorProvider(() => sl);

			objectFactory.Register<IInvoiceTotalCalculator, SumAndTaxTotalCalculator>();
			objectFactory.RegisterPrototype<IInvoice, Invoice>();
		}

		protected override IBytecodeProvider GetBytecodeProvider()
		{
			return new EnhancedBytecode(objectFactory);
		}
	}
}