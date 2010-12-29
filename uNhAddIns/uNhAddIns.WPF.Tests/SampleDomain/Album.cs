using System.Collections.Generic;

namespace uNhAddIns.WPF.Tests.SampleDomain
{
    public class Album 
    {
        public virtual int Id { get; set; }
        public Album()
        {
            Tracks = new List<Track>();
        }

        public virtual string Title { get; set; }
        public virtual IList<Track> Tracks { get; private set; }

        public virtual void AddTrack(Track track)
        {
            track.Album = this;
            Tracks.Add(track);
        }
    }
}