namespace YourPrjDomain.Filters
{
	public class Person
	{
		private int id;
		private string name;
		private bool live;

		public Person(string name)
		{
			this.name = name;
		}

		public Person() {}

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		public bool Live
		{
			get { return live; }
			set { live = value; }
		}
	}
}
