using System;
using Microsoft.Practices.ServiceLocation;
using NHibernate.Tuple;

namespace uNHAddIns.Examples.CustomInterceptor.Infrastructure
{

    /// <summary>
    /// Resolve an entity through ServiceLocator.
    /// This allow to inject a proxy and intercepted entity.
    /// </summary>
    public class ServiceLocatorInstantiator : IInstantiator
    {
        #region IInstantiator Members

        private readonly Type _entityType;

        public ServiceLocatorInstantiator(Type entityType)
        {
            _entityType = entityType;
        }

        public object Instantiate(object id)
        {
            var obj = ServiceLocator.Current.GetInstance(_entityType);
            return obj;
        }

        public object Instantiate()
        {
            return Instantiate(null);
        }

        public bool IsInstance(object obj)
        {
            return _entityType.IsInstanceOfType(obj);
        }

        #endregion
    }
}