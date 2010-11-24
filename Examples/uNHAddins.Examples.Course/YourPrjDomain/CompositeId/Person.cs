namespace YourPrjDomain.CompositeId
{
	public class Person
	{
		private string dni;
		private string name;

		public Person() {}

		public Person(string dni, string name)
		{
			this.dni = dni;
			this.name = name;
		}

		public string Dni
		{
			get { return dni; }
			set { dni = value; }
		}

		public string Name
		{
			get { return name; }
			set { name = value; }
		}
	}
}
