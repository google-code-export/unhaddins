using System;
using Microsoft.Practices.ServiceLocation;
using NHibernate.Bytecode.Lightweight;
using NHibernate.Properties;

namespace uNhAddIns.CommonServiceLocatorAdapters.BytecodeProvider
{
  public class CSLReflectionOptimizer : ReflectionOptimizer
  {

    public CSLReflectionOptimizer(
    Type mappedType, IGetter[] getters, ISetter[] setters)
      : base(mappedType, getters, setters)
    { }

    /// <summary>
    /// Ignore this check
    /// </summary>
    /// <param name="type"></param>
    protected override void ThrowExceptionForNoDefaultCtor(Type type)
    {
    }

    public override object CreateInstance()
    {
      return ServiceLocator.Current.GetInstance(mappedType);
    }

  }
}
