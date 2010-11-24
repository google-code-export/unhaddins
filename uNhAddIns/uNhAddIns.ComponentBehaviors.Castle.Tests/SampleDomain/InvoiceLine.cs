using uNhAddIns.Entities;

namespace uNhAddIns.ComponentBehaviors.Castle.Tests.SampleDomain
{
    public class InvoiceLine : Entity
    {
        public virtual Invoice Invoice { get; set; }
    }
}