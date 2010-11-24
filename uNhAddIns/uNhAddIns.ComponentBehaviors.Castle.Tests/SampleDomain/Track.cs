using uNhAddIns.Entities;

namespace uNhAddIns.ComponentBehaviors.Castle.Tests.SampleDomain
{
    public class Track : Entity
    {
        public virtual string Name { get; set; }
        public virtual Album Album { get; set; }
    }
}