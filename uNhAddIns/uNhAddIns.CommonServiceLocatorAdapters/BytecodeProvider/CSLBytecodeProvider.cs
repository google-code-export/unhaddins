using System;
using NHibernate.Bytecode;
using NHibernate.Properties;
using NHibernate.Type;

namespace uNhAddIns.CommonServiceLocatorAdapters.BytecodeProvider
{
  public class CSLBytecodeProvider :
    IBytecodeProvider,
    IInjectableCollectionTypeFactoryClass
  {

    public CSLBytecodeProvider()
    {
      _objectsFactory = new CSLObjectsFactory();
      _collectionTypeFactory = new DefaultCollectionTypeFactory();
    }

    protected readonly IObjectsFactory _objectsFactory;
    protected ICollectionTypeFactory _collectionTypeFactory;

    public ICollectionTypeFactory CollectionTypeFactory
    {
      get { return _collectionTypeFactory; }
    }

    public IReflectionOptimizer GetReflectionOptimizer(
      Type clazz, IGetter[] getters, ISetter[] setters)
    {
      return new CSLReflectionOptimizer(clazz, getters, setters);
    }

    public IObjectsFactory ObjectsFactory
    {
      get { return _objectsFactory; }
    }

    public IProxyFactoryFactory ProxyFactoryFactory
    {
      get { return new CSLProxyFactoryFactory(); }
    }

    public void SetCollectionTypeFactoryClass(Type type)
    {
      _collectionTypeFactory =
        (ICollectionTypeFactory)
        ObjectsFactory.CreateInstance(type);
    }

    public void SetCollectionTypeFactoryClass(
      string typeAssemblyQualifiedName)
    {
      var type = Type.GetType(typeAssemblyQualifiedName);
      SetCollectionTypeFactoryClass(type);
    }

  }
}
