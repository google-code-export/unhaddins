using System;
using NHibernate;
using NHibernate.Context;

namespace uNhAddIns.ComponentBehaviors.Castle.Tests
{
    public static class SessionFactoryExtension
    {
        public static void InTransaction(
            this ISessionFactory sessionFactory,
            Action<ISession,ITransaction> action)
        {
            using(var session = sessionFactory.OpenSession())
            using(var tx = session.BeginTransaction())
            {
                CurrentSessionContext.Bind(session);

                action(session, tx);

                CurrentSessionContext.Unbind(sessionFactory);

                if(tx.IsActive)
                    tx.Commit();
            }
        }
    }
}