namespace uNhAddIns.WPF.Tests.SampleDomain
{
    public class Track 
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual Album Album { get; set; }
    }
}