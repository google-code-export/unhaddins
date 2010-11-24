using System.Collections.Generic;
using uNhAddIns.Entities;

namespace uNhAddIns.ComponentBehaviors.Castle.Tests.SampleDomain
{
    public class Album : Entity
    {
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