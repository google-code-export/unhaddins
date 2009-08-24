﻿using System.Linq;
using Castle.Core;
using Castle.MicroKernel.Registration;
using NHibernate;
using NUnit.Framework;
using uNhAddIns.WPF.Castle;
using uNhAddIns.WPF.Castle.EntityNameResolver;
using uNhAddIns.WPF.Tests.SampleDomain;

namespace uNhAddIns.WPF.Tests.Castle
{
    [TestFixture]
    public class EntityNameResolverFixture : IntegrationBaseTest
    {
        protected override void ConfigureWindsorContainer()
        {
            container.Register(Component.For<EditableBehaviorInterceptor>()
                                   .LifeStyle.Transient);

            container.Register(Component.For<GetEntityNameInterceptor>()
                                   .LifeStyle.Transient);

            container.Register(Component.For<Album>()
                                   .Proxy.AdditionalInterfaces(typeof (INamedEntity))
                                   .Interceptors(new InterceptorReference(typeof (GetEntityNameInterceptor))).Anywhere
                                   .LifeStyle.Transient);
        }

        private int CreateNewAlbum()
        {
            int id;
            using (ISession session = sessions.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                var album = new Album();
                album.Title = "The dark side of the moon";
                id = (int) session.Save(album);
                tx.Commit();
            }
            return id;
        }

        [Test]
        public void can_attach_nontransient_entity()
        {
            int idAlbum = CreateNewAlbum();
            Album album;

            using (ISession session = sessions.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                album = session.Get<Album>(idAlbum);
                NHibernateUtil.Initialize(album.Tracks);
                tx.Commit();
            }

            using (ISession session = sessions.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                album.Title = "dark side";
                session.Update(album);
                tx.Commit();
            }

            using (ISession session = sessions.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                session.Get<Album>(idAlbum).Title.Should().Be.EqualTo("dark side");
                tx.Commit();
            }
        }


        [Test]
        public void can_merge_entity()
        {
            int idAlbum = CreateNewAlbum();
            Album album;

            using (ISession session = sessions.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                album = session.Get<Album>(idAlbum);
                tx.Commit();
            }

            using (ISession session = sessions.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                var mergedAlbum = (Album) session.Merge(album);
                mergedAlbum.Title = "dark side";
                tx.Commit();
            }

            using (ISession session = sessions.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                album = session.Get<Album>(idAlbum);
                album.Title.Should().Be.EqualTo("dark side");
                tx.Commit();
            }
        }


        [Test]
        public void can_merge_with_an_already_loaded_entity()
        {
            int idAlbum = CreateNewAlbum();
            Album album;

            using (ISession session = sessions.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                album = session.Get<Album>(idAlbum);
                tx.Commit();
            }

            using (ISession session = sessions.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                session.Load<Album>(idAlbum);
                var mergedAlbum = (Album) session.Merge(album);
                mergedAlbum.Title = "dark side";
                tx.Commit();
            }

            using (ISession session = sessions.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                album = session.Get<Album>(idAlbum);
                album.Title.Should().Be.EqualTo("dark side");
                tx.Commit();
            }
        }

        [Test]
        public void can_save_transient_entity()
        {
            using (ISession session = sessions.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                var album = container.Resolve<Album>();
                session.Save(album);
                tx.Commit();
            }
        }

        [Test]
        public void can_persist_transient_entity()
        {
            using (ISession session = sessions.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                var album = container.Resolve<Album>();
                session.Persist(album);
                tx.Commit();
            }
        }

        // TODO: EnhacedProxyFactory.
        [Test]
        [Ignore(@"this test should not be here. 
                 I need to build an EnhancedProxyFactory")]
        public void loaded_entity_implements_INamedEntity()
        {
            int id = CreateNewAlbum();
            using (ISession session = sessions.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                var album = session.Load<Album>(id);
                NHibernateUtil.Initialize(album);
                album.GetType().GetInterfaces()
                    .Any(i => i.Equals(typeof (INamedEntity))).Should().Be.True();
            }
        }
    }
}