namespace YourPrjDomain.OptimisticLock
{
	public class Person
	{
		private string name;
		private int version;
		private int id;

		public virtual int Id
		{
			get { return id; }
			set { id = value; }
		}

		protected Person()
		{
		}

		public Person(string name)
		{
			this.name = name;
		}

		public virtual string Name
		{
			get { return name; }
			set { name = value; }
		}

		public virtual int Version
		{
			get { return version; }
			set { version = value; }
		}
	}
}
