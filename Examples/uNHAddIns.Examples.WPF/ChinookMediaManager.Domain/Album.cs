using System.Collections.Generic;
using Iesi.Collections.Generic;
using uNhAddIns.Entities;

namespace ChinookMediaManager.Domain
{
	public class Album : Entity
	{
		private readonly ISet<Track> tracks;

		public Album()
		{
			tracks = new HashedSet<Track>();
		}

		public virtual Artist Artist { get; set; }

		public virtual string Title { get; set; }

		public virtual ICollection<Track> Tracks
		{
			get { return tracks; }
		}

		public virtual void AddTrack(Track track)
		{
			track.Album = this;
			Tracks.Add(track);
		}

		public virtual void RemoveTrack(Track track)
		{
			track.Album = null;
			Tracks.Remove(track);
		}

		public override string ToString()
		{
			return Title;
		}
	}
}