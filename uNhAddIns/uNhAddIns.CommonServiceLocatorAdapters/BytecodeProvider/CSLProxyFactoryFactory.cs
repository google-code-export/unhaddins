using System;
using Microsoft.Practices.ServiceLocation;
using NHibernate.Bytecode;
using NHibernate.Proxy;
using uNhAddIns.CommonServiceLocatorAdapters.Proxy;

namespace uNhAddIns.CommonServiceLocatorAdapters.BytecodeProvider
{
  public class CSLProxyFactoryFactory : IProxyFactoryFactory
  {

    public IProxyFactory BuildProxyFactory()
    {
      return ServiceLocator.Current.GetInstance<IProxyFactory>();
    }

    public bool IsInstrumented(Type entityClass)
    {
      return false;
    }

    public IProxyValidator ProxyValidator
    {
      get
      {
        return new ProxyTypeValidator();
      }
    }

  }
}
