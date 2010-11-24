namespace YourPrjDomain.EventListeners
{
	public class Person
	{
		public Person()
		{
		}
		
		public Person(string firstName, string lastName)
		{
			this.firstName = firstName;
			this.lastName = lastName;
		}

		private int id;

		public virtual int Id
		{
			get { return id; }
			set { id = value; }
		}

		private string firstName;

		public virtual string FirstName
		{
			get { return firstName; }
			set { firstName = value; }
		}

		private string lastName;

		public virtual string LastName
		{
			get { return lastName; }
			set { lastName = value; }
		}

		public override string ToString()
		{
			return string.Format("Id {0}: {1} {2}", Id, FirstName, LastName);
		}
	}
}