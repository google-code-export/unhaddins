using System.Collections.Specialized;
using System.ComponentModel;
using NHibernate;
using NUnit.Framework;
using uNhAddIns.WPF.Collections;
using uNhAddIns.WPF.Collections.PersistentImpl;
using uNhAddIns.WPF.Tests.SampleDomain;

namespace uNhAddIns.WPF.Tests.Collections
{
    [TestFixture]
    public class ObservablePersistentGenericSetFixture : IntegrationBaseTest
    {
        private int CreateNewPlayList()
        {
            int id;
            using (ISession session = sessions.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                var track = new Track{Name = "Shine"};
                session.Save(track);

                var playList = new Playlist();
                playList.AddTrack(track);
                playList.Name = "pelotas playlist";
                id = (int)session.Save(playList);
                tx.Commit();
            }
            return id;
        }
        
        [Test]
        public void collection_should_implement_notificables()
        {
            using (ISession session = sessions.OpenSession())
            {
                var id = CreateNewPlayList();
                var playlist = session.Get<Playlist>(id);

                playlist.Tracks.GetType().Should().Be.AssignableTo(typeof(INotifyCollectionChanged));
                playlist.Tracks.GetType().Should().Be.AssignableTo(typeof(INotifyPropertyChanged));
            }
        }


        [Test]
        public void add_item_should_raise_collectionchanged()
        {
            using (ISession session = sessions.OpenSession())
            {
                var id = CreateNewPlayList();
                bool eventWasRaised = false;
                var playlist = session.Get<Playlist>(id);
                ((INotifyCollectionChanged) playlist.Tracks)
                    .CollectionChanged += (sender, args) =>
                    {
                        sender.Should().Be.EqualTo(
                            playlist.Tracks);
                        args.NewItems.Count.Should().
                            Be.EqualTo(1);
                        eventWasRaised = true;
                    };

                var track = new Track { Name = "what a wonderful world" };
                playlist.AddTrack(track);
                eventWasRaised.Should().Be.True();
            }
        }
        
        //TODO: Find the way to inherit
    }
}