using NHibernate;

namespace uNhAddIns.NHibernateTypeResolver
{
    //note: The idea is to use just Events. 
    //But in the ISession.Load with object and ISession.Refresh 
    //we cann't change the entity name.

    public class EntityNameInterceptor : EmptyInterceptor
    {
        public override string GetEntityName(object entity)
        {
            if (entity is IWellKnownProxy)
            {
                var namedEntity = (IWellKnownProxy) entity;
                return namedEntity.EntityName;
            }

            return base.GetEntityName(entity);
        }
    }
}