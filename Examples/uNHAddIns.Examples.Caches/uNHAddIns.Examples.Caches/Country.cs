namespace uNHAddIns.Examples.Caches
{
	public class Country
	{
		public Country()
		{
		}

		public Country(string name, string continentName)
		{
			this.name = name;
			this.continentName = continentName;
		}

		private int id;

		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		private string name;

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		private string continentName;

		public string ContinentName
		{
			get { return continentName; }
			set { continentName = value; }
		}
	}
}