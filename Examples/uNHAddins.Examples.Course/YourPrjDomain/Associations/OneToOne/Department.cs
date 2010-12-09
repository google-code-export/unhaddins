namespace YourPrjDomain.Associations.OneToOne
{
	public class Department
	{
		private int id;
		private Manager managedBy;
		private string name;

		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public Manager ManagedBy
		{
			get { return managedBy; }
			set { managedBy = value; }
		}
	}
}