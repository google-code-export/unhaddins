using System.ComponentModel;
using NHibernate;
using NHibernate.Context;
using NUnit.Framework;
using uNhAddIns.ComponentBehaviors.Castle.Configuration;
using uNhAddIns.ComponentBehaviors.Castle.Tests.SampleDomain;
using Component=Castle.MicroKernel.Registration.Component;

namespace uNhAddIns.ComponentBehaviors.Castle.Tests.Fixes
{
    [TestFixture]
    public class CBTest1 : IntegrationBaseTest
    {
        protected override void ConfigureWindsorContainer()
        {
            base.ConfigureWindsorContainer();

            container.Register(Component.For<EditableBehavior>());
            container.Register(Component.For<GetEntityNameBehavior>());
            container.Register(Component.For<NotifyPropertyChangedBehavior>());

            container.Register(Component.For<AlreadyNamedEntity>()
                                   .AddNotificableBehavior()
                                   .AddEditableBehavior()
                                   .NhibernateEntity()
                                   .LifeStyle.Transient);
        }

        [Test]
        public void can_save_new_named_entity()
        {
            using (ISession session = sessions.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                CurrentSessionContext.Bind(session);

                var namedEntity = container.Resolve<AlreadyNamedEntity>();

                namedEntity.Name = "Sunny";
                
                session.Persist("namedEntity", namedEntity);
                tx.Commit();
            }
        }
    }
}