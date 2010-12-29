using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Bytecode;
using Microsoft.Practices.ServiceLocation;

namespace uNhAddIns.CommonServiceLocatorAdapters.BytecodeProvider
{
  public class CSLObjectsFactory : IObjectsFactory
  {

    private readonly ActivatorObjectsFactory _activatorObjectFactory;

    public CSLObjectsFactory()
    {
      _activatorObjectFactory = new ActivatorObjectsFactory();
    }

    public object CreateInstance(Type type, params object[] ctorArgs)
    {
      return _activatorObjectFactory.CreateInstance(
        type, ctorArgs);
    }

    public object CreateInstance(Type type, bool nonPublic)
    {
      return _activatorObjectFactory.CreateInstance(
        type, nonPublic);
    }

    public object CreateInstance(Type type)
    {
      try
      {
        return ServiceLocator.Current.GetInstance(type);
      }
      catch
      {
        return _activatorObjectFactory.CreateInstance(type);
      }
    }

  }
}
