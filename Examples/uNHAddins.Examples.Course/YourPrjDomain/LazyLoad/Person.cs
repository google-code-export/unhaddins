using System.Collections.Generic;

namespace YourPrjDomain.LazyLoad
{
	public class Person
	{
		private IList<Account> accounts = new List<Account>();
		private int id;
		private string name;

		public virtual int Id
		{
			get { return id; }
			set { id = value; }
		}

		public virtual string Name
		{
			get { return name; }
			set { name = value; }
		}

		public virtual IList<Account> Accounts
		{
			get { return accounts; }
			set { accounts = value; }
		}
	}
}