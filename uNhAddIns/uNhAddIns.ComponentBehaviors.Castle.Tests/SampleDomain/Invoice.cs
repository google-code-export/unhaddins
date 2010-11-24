using System.Collections.Generic;
using uNhAddIns.Entities;

namespace uNhAddIns.ComponentBehaviors.Castle.Tests.SampleDomain
{
    public class Invoice : Entity
    {
        public virtual IList<InvoiceLine> Lines { get; private set; }
        public virtual void AddLine(InvoiceLine invoiceLine)
        {
            invoiceLine.Invoice = this;
            Lines.Add(invoiceLine);
        }
        
        public virtual void RemoveLine(InvoiceLine invoiceLine)
        {
            //invoiceLine.Invoice = null;
            Lines.Remove(invoiceLine);
        }



        public Invoice()
        {
            Lines = new List<InvoiceLine>();
        }
    }
}