
using System.Linq;
using NHibernate.Event;

namespace uNhAddIns.NHibernateTypeResolver
{
    public static class ConfigurationExtension
    {
        public static NHibernate.Cfg.Configuration RegisterEntityNameResolver(this NHibernate.Cfg.Configuration configuration)
        {
            EventListeners listeners = configuration.EventListeners;
            var entityNameResolver = new EntityNameResolver();
            listeners.MergeEventListeners =
                new[] {entityNameResolver}.Concat(listeners.MergeEventListeners).ToArray();
            listeners.UpdateEventListeners =
                new[] {entityNameResolver}.Concat(listeners.UpdateEventListeners).ToArray();
            listeners.SaveOrUpdateEventListeners =
                new[] {entityNameResolver}.Concat(listeners.SaveOrUpdateEventListeners).ToArray();
            listeners.SaveEventListeners =
                new[] {entityNameResolver}.Concat(listeners.SaveEventListeners).ToArray();
            listeners.PersistEventListeners =
                new[] {entityNameResolver}.Concat(listeners.PersistEventListeners).ToArray();

            configuration.Interceptor = new EntityNameInterceptor();
            return configuration;
        }
    }
}