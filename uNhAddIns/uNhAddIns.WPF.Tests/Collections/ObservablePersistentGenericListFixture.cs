using System;
using System.Collections.Specialized;
using System.ComponentModel;
using NHibernate;
using NUnit.Framework;
using uNhAddIns.WPF.Tests.SampleDomain;

namespace uNhAddIns.WPF.Tests.Collections
{
    [TestFixture]
    public class ObservablePersistentGenericListFixture : IntegrationBaseTest
    {
        /// <summary>
        /// Create and store an album with two tracks.
        /// </summary>
        /// <returns></returns>
        private int CreateNewAlbum()
        {
            int id;
            using (ISession session = sessions.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                var album = new Album {Title = "Aftermath"};

                album.AddTrack(new Track()
                                   {
                                       Name = "Paint it black"
                                   });
                album.AddTrack(new Track()
                                    {
                                        Name = "Stupid Girl"
                                    });

                id = (int)session.Save(album);
                tx.Commit();
            }
            return id;
        }

        [Test]
        public void add_item_should_raise_collectionchanged_properly()
        {
            int id = CreateNewAlbum();
            bool eventWasRaised = false;

            using (ISession session = sessions.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                var album = session.Get<Album>(id);
                var newTrack = new Track() {Name = "Lady Jane"};

                ((INotifyCollectionChanged) album.Tracks).CollectionChanged 
                    += (sender, args) =>{
                                        sender.Should().Be.EqualTo(
                                            album.Tracks);
                                        args.Action.Should().Be.EqualTo(
                                            NotifyCollectionChangedAction
                                                .Add);
                                        args.NewItems.Count.Should().Be.
                                            EqualTo(1);
                                        args.NewItems[0].Should().Be.
                                            EqualTo(newTrack);
                                        eventWasRaised = true;
                                                                                    };

                album.AddTrack(newTrack);
                eventWasRaised.Should().Be.True();
                tx.Commit();
            }
        }

        [Test]
        public void add_item_should_raise_propertychanged_for_count()
        {
            int id = CreateNewAlbum();
            bool eventWasRaised = false;

            using (ISession session = sessions.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                var album = session.Get<Album>(id);
                var newTrack = new Track { Name = "Lady Jane" };

                ((INotifyPropertyChanged)album.Tracks).PropertyChanged += (sender, args) =>
                                                                                {
                                                                                    if (args.PropertyName == "Count")
                                                                                        eventWasRaised = true;
                                                                                };

                album.AddTrack(newTrack);
                eventWasRaised.Should().Be.True();
                tx.Commit();
            }
        }

        [Test]
        public void add_item_should_raise_propertychanged_for_item()
        {
            int id = CreateNewAlbum();
            bool eventWasRaised = false;

            using (ISession session = sessions.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                var album = session.Get<Album>(id);
                var newTrack = new Track();

                ((INotifyPropertyChanged) album.Tracks).PropertyChanged += (sender, args) =>
                                                                                {
                                                                                    if (args.PropertyName == "Item[]")
                                                                                        eventWasRaised = true;
                                                                                };

                album.AddTrack(newTrack);
                eventWasRaised.Should().Be.True();
                tx.Commit();
            }
        }

        [Test]
        public void collection_should_implement_inotifycollectionchanged()
        {
            int id = CreateNewAlbum();
            using (ISession session = sessions.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                var album = session.Get<Album>(id);
                typeof (INotifyCollectionChanged)
                    .Should().Be.AssignableFrom(album.Tracks.GetType());
                tx.Commit();
            }
        }

        [Test]
        public void collection_should_implement_inotifypropertychanged()
        {
            int id = CreateNewAlbum();
            using (ISession session = sessions.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                var album = session.Get<Album>(id);
                typeof(INotifyPropertyChanged)
                    .Should().Be.AssignableFrom(album.Tracks.GetType());
                tx.Commit();
            }
        }
        
    }
}