using System.Collections.Generic;

namespace uNhAddIns.WPF.Tests.SampleDomain
{
    public class Invoice 
    {
        public virtual int Id { get; set; }
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