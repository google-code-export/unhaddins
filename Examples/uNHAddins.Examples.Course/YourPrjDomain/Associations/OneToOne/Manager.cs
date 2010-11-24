namespace YourPrjDomain.Associations.OneToOne
{
	public class Manager
	{
		private string fullName;
		private int id;
		private Department manageTo;


		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		public string FullName
		{
			get { return fullName; }
			set { fullName = value; }
		}

		public Department ManageTo
		{
			get { return manageTo; }
			set { manageTo = value; }
		}
	}
}