using System.Linq;
using ChinookMediaManager.Data.Impl;
using ChinookMediaManager.Data.Test.BaseClasses;
using ChinookMediaManager.Domain;
using NHibernate.Context;
using NUnit.Framework;
using SharpTestsEx;

namespace ChinookMediaManager.Data.Test
{
	[TestFixture]
	public class AlbumDataTest : PersistenceFixtureBase
	{

		
		[Test(Description = "This test ensure the cascades are working properly.")]
		public void WhenDeletingAnAlbumThenDeleteAllTracks()
		{
			//CommitInNewSession is the same that using(session)ussing(tx)... tx.commit.

			Album album = CreateAnAlbumWithTwoTracks();
			CommitInNewSession(s => s.Save(album));
			CommitInNewSession(s => s.Delete(album));
			CommitInNewSession(s => s.CreateQuery("from Track").List<Track>().Should().Be.Empty());
		}


		[Test]
		public void CanSaveAndRead()
		{
			Album album;

			using (var session = Sessions.OpenSession())
			using (var tx = session.BeginTransaction())
			{
				album = CreateAnAlbumWithTwoTracks();
                               
				session.Save(album);
				tx.Commit();
			}

			using (var session = Sessions.OpenSession())
			using (var tx = session.BeginTransaction())
			{

				var fromDb = session.Get<Album>(album.Id);
				fromDb.Title.Should().Be.EqualTo(album.Title);
				fromDb.Tracks.Count.Should().Be.EqualTo(2);
				fromDb.Tracks.Satisfy(
					tracks => tracks.Any(t => t.Name == "Sample Song1") && tracks.Any(t => t.Name == "Sample Song2"));
				tx.Commit();
			}
		}


		/// <summary>
		/// Usually I put this method in other class... It is a test Scenario.
		/// </summary>
		/// <returns></returns>
		private static Album CreateAnAlbumWithTwoTracks()
		{
			var album = new Album {Title = "SAmple Album"};
			album.AddTrack(new Track{Name = "Sample Song1"});
			album.AddTrack(new Track { Name = "Sample Song2" });
			return album;
		}

		protected override void OnTearDown()
		{
			using (var session = Sessions.OpenSession())
			using (var tx = session.BeginTransaction())
			{
				session.Delete("from Album");
				tx.Commit();
			}
		}

	}
}