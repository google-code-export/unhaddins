using System.Collections.Generic;
using log4net;
using NHibernate;


namespace uNhAddIns.Example.MonoRailConversationUsage.DatabaseAccessObjects {
    public class BaseDao<TEntity> : IDao<TEntity>  {
        readonly ILog log = LogManager.GetLogger(typeof (BaseDao<>));

        protected readonly ISessionFactory factory;

        public BaseDao(ISessionFactory factory) {
            this.factory = factory;
        }

        public TEntity MakePersistent(TEntity entity) {
            // HACK: for test purpose only
            var obj1 = entity as IWithSessionId;
            if (obj1 != null) {
                obj1.NHibernateSessionId = ObtainSession().GetHashCode().ToString();
            }
            ObtainSession().SaveOrUpdate(entity);
            return entity;
        }

        public void MakeTransient(TEntity entity) {
            ObtainSession().Delete(entity);
        }

        ISession ObtainSession() {
            var session = factory.GetCurrentSession();
            log.Debug("Session " + session.GetHashCode() + " obtained.");

            return session;
        }

        public IList<TEntity> FindAll() {  
            ICriteria criteria = factory.GetCurrentSession().CreateCriteria(typeof(TEntity));
            return criteria.List<TEntity>();
        }

        public TEntity TryFind(int id) {
            return ObtainSession().Get<TEntity>(id);
        }

    }


}