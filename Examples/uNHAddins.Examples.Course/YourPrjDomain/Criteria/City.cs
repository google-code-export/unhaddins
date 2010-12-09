namespace YourPrjDomain.Criteria
{
	public class City
	{
		private string name;
		private string zip;

		public City()
		{
		}

		public City(string name, string zip)
		{
			this.name = name;
			this.zip = zip;
		}

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public string Zip
		{
			get { return zip; }
			set { zip = value; }
		}
	}
}