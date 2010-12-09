using System.Collections.Generic;

namespace YourPrjDomain.Associations.ManyToMany
{
	public class Category
	{
		private string description;
		private int id;
		private IList<Item> items = new List<Item>();

		public Category()
		{
		}

		public Category(string description)
		{
			this.description = description;
		}

		public int Id
		{
			set { id = value; }
			get { return id; }
		}

		public IList<Item> Items
		{
			get { return items; }
			set { items = value; }
		}

		public string Description
		{
			get { return description; }
			set { description = value; }
		}
	}
}