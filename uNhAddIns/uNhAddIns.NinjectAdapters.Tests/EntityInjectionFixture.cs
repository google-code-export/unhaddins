using Ninject;
using Microsoft.Practices.ServiceLocation;
using NHibernate.Proxy;
using NHibernate.Bytecode;
using NHibernate.ByteCode.Castle;
using NUnit.Framework;
using uNhAddIns.Adapters.CommonTests.EnhancedBytecodeProvider;
using uNhAddIns.NinjectAdapters.BytecodeProvider;

namespace uNhAddIns.NinjectAdapters.Tests
{
    [TestFixture]
    public class EntityInjectionFixture : AbstractEntityInjectionFixture
    {
        private IKernel kernel;

        protected override void InitializeServiceLocator()
        {
            kernel = new StandardKernel();
            kernel.Bind<IProxyFactory>().To<ProxyFactory>();
            kernel.Bind<IInvoiceTotalCalculator>().To<SumAndTaxTotalCalculator>();
            kernel.Bind<IInvoice>().To<Invoice>().InTransientScope();
            IServiceLocator sl = new NinjectServiceLocator(kernel);
            ServiceLocator.SetLocatorProvider(() => sl);
            kernel.Bind<IServiceLocator>().ToConstant(sl);
        }

        protected override IBytecodeProvider GetBytecodeProvider()
        {
            return new NinjectBytecodeProvider(kernel);
        }
    }
}