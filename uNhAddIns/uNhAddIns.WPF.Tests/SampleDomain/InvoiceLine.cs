namespace uNhAddIns.WPF.Tests.SampleDomain
{
    public class InvoiceLine
    {
        public virtual int Id { get; set; }
        public virtual Invoice Invoice { get; set; }
    }
}