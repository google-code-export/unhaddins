using System;
using NHibernate.Validator.Engine;

namespace uNhAddIns.NHibernateTypeResolver
{
    //NHibernate Validator Type Inspector.

    public class NHVTypeInspector : IEntityTypeInspector
    {
        public Type GuessType(object entityInstance)
        {
            var entity = entityInstance as IWellKnownProxy;
            if (entity != null)
                return entity.EntityType;
            return null;
        }
    }
}