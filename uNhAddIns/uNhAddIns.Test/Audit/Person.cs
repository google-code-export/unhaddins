using System.Collections.Generic;

namespace uNhAddIns.Test.Audit
{
	public class Person
	{
		public virtual int Id { get; set; }
		public virtual string Identification { get; set; }
		public virtual string Name { get; set; }
		public virtual Address ActualAddress { get; set; }
		public virtual ICollection<string> Nicks { get; set; }
		public virtual IList<Animal> Pets { get; set; }
	}
}