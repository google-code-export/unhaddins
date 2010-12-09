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
  public class InjectableUserTypeFixture
    : AbstractInjectableUserTypeFixture
  {

    /// - uNhAddIns.Adapters.CommonTests.EnhancedBytecodeProvider.IDelimiter 
    ///		Implementation: uNhAddIns.Adapters.CommonTests.EnhancedBytecodeProvider.ParenDelimiter
    /// 
    /// - uNhAddIns.Adapters.CommonTests.EnhancedBytecodeProvider.InjectableStringUserType 
    protected override void InitializeServiceLocator()
    {
      var kernel = new StandardKernel();
      kernel.Bind<IProxyFactory>()
        .To<ProxyFactory>();
      kernel.Bind<IDelimiter>()
        .To<ParenDelimiter>();
      kernel.Bind<InjectableStringUserType>()
        .ToSelf();
      IServiceLocator sl = new NinjectServiceLocator(kernel);
      ServiceLocator.SetLocatorProvider(() => sl);
    }

    protected override IBytecodeProvider GetBytecodeProvider()
    {
      return new CSLBytecodeProvider();
    }

  }
}
