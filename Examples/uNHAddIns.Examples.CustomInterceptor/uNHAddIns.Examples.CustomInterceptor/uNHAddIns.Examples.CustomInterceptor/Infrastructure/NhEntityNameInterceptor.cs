using NHibernate;

namespace uNHAddIns.Examples.CustomInterceptor.Infrastructure
{
    public class NhEntityNameInterceptor : EmptyInterceptor
    {
        public override string GetEntityName(object entity)
        {
            var proxyInfo = entity as IProxiedEntity;
            return proxyInfo != null ? proxyInfo.EntityName : base.GetEntityName(entity);
        }
        
    }
}