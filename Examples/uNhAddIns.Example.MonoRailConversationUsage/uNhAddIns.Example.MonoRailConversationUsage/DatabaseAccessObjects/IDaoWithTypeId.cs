using System.Collections.Generic;
using NHibernate;

namespace uNhAddIns.Example.MonoRailConversationUsage.DatabaseAccessObjects {
    public interface IDaoWithTypeId<TEntity, TIdType>{
        TEntity TryFind(TIdType id);
        TEntity MakePersistent(TEntity entity);
        void MakeTransient(TEntity entity);
        IList<TEntity> FindAll();
    }
}