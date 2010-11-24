using System.ComponentModel;
using Castle.Core;
using NHibernate;
using NUnit.Framework;
using uNhAddIns.ComponentBehaviors.Castle.Tests.SampleDomain;
using uNhAddIns.NHibernateTypeResolver;
using Component=Castle.MicroKernel.Registration.Component;

namespace uNhAddIns.ComponentBehaviors.Castle.Tests
{
    [TestFixture]
    public class EditableBehaviorInterceptorFixture : IntegrationBaseTest
    {
        protected override void ConfigureWindsorContainer()
        {
            container.Register(Component.For<EditableBehavior>()
                                   .LifeStyle.Transient);

            container.Register(Component.For<GetEntityNameBehavior>()
                                   .LifeStyle.Transient);

            container.Register(Component.For<Album>()
                                   .Proxy.AdditionalInterfaces(typeof (IEditableObject), typeof (IWellKnownProxy))
                                   .Interceptors(new InterceptorReference(typeof (EditableBehavior))).
                                   Anywhere
                                   .Interceptors(new InterceptorReference(typeof (GetEntityNameBehavior))).Anywhere
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
        public void entity_should_implement_ieditableobject()
        {
            var id = CreateNewAlbum();
            using (ISession session = sessions.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                var album = session.Get<Album>(id);
                album.GetType().GetInterfaces().Should().Contain(typeof (IEditableObject));
                tx.Commit();
            }
        }

        [Test]
        public void get_on_readonly_property_should_work()
        {
            var album = container.Resolve<Album>();
            ((IEditableObject) album).BeginEdit();
            album.Tracks.Should().Not.Be.Null();
            ((IEditableObject) album).CancelEdit();
        }

        [Test]
        public void session_should_be_dirty_after_commitchanges()
        {
            var id = CreateNewAlbum();
            using (ISession session = sessions.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                var album = session.Get<Album>(id);
                ((IEditableObject) album).BeginEdit();
                album.Title = "Dark side of the moon";
                ((IEditableObject) album).EndEdit();
                session.IsDirty().Should().Be.True();
                tx.Commit();
            }
        }

        [Test]
        public void session_shouldnot_be_dirty_after_cancelchanges()
        {
            var id = CreateNewAlbum();
            using (ISession session = sessions.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                var album = session.Get<Album>(id);
                ((IEditableObject) album).BeginEdit();
                album.Title = "Dark side of the moon";
                ((IEditableObject) album).CancelEdit();
                session.IsDirty().Should().Be.False();

                tx.Commit();
            }
        }

        [Test]
        public void tempvalues_should_not_traverse_session()
        {
            var album = container.Resolve<Album>();
            const string title = "The dark side of the moon";
            album.Title = title;

            ((IEditableObject) album).BeginEdit();
            album.Title = "dark side";
            ((IEditableObject) album).CancelEdit();

			((IEditableObject) album).BeginEdit();
            album.Title.Should().Be.EqualTo(title);
            ((IEditableObject) album).CancelEdit();
        }

        [Test]
        public void transient_entity_should_commitchanges_after_endedit()
        {
            var album = container.Resolve<Album>();
            const string title = "The dark side of the moon";
            const string newTitle = "Dark side of the moon";

            album.Title = title;
            ((IEditableObject) album).BeginEdit();
            album.Title = newTitle;
            album.Title.Should().Be.EqualTo(newTitle);
            ((IEditableObject) album).EndEdit();

            album.Title.Should().Be.EqualTo(newTitle);
        }


        [Test]
        public void transient_entity_should_rollback_after_canceledit()
        {
            var album = container.Resolve<Album>();
            const string title = "The dark side of the moon";
            const string newTitle = "Dark side of the moon";

            album.Title = title;
            ((IEditableObject) album).BeginEdit();
            album.Title = newTitle;
            album.Title.Should().Be.EqualTo(newTitle);
            ((IEditableObject) album).CancelEdit();

            album.Title.Should().Be.EqualTo(title);
        }


		[Test]
		public void null_values_in_edit_session_should_work()
		{
			var album = container.Resolve<Album>();

			album.Title = "The dark side of the moon";

			((IEditableObject)album).BeginEdit();
			album.Title = null;
			album.Title.Should().Be.EqualTo(null);
			((IEditableObject)album).EndEdit();
            
		}
    }
}