using Microsoft.Practices.ServiceLocation;
using NHibernate.Bytecode;
using NHibernate.ByteCode.Castle;
using NHibernate.Proxy;
using Ninject;
using NUnit.Framework;
using uNhAddIns.Adapters.CommonTests.EnhancedBytecodeProvider;
using uNhAddIns.CommonServiceLocatorAdapters.BytecodeProvider;
using uNhAddIns.NinjectAdapters.Tests;

namespace uNhAddIns.CommonServiceLocatorAdapters.Tests
{
    [TestFixture]
    public class EntityInjectionFixture : AbstractEntityInjectionFixture
    {


        protected override void InitializeServiceLocator()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IProxyFactory>()
              .To<ProxyFactory>();
            kernel.Bind<IInvoiceTotalCalculator>()
              .To<SumAndTaxTotalCalculator>();
            kernel.Bind<IInvoice>()
              .To<Invoice>().InTransientScope();
            IServiceLocator sl = new NinjectServiceLocator(kernel);
            ServiceLocator.SetLocatorProvider(() => sl);
        }

        protected override IBytecodeProvider GetBytecodeProvider()
        {
            return new CSLBytecodeProvider();
        }
    }
}