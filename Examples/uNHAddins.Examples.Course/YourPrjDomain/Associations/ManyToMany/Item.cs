using System.Collections.Generic;

namespace YourPrjDomain.Associations.ManyToMany
{
	public class Item
	{
		private IList<Category> categories = new List<Category>();
		private string description;
		private int id;

		public Item()
		{
		}

		public Item(string description)
		{
			this.description = description;
		}

		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		public IList<Category> Categories
		{
			get { return categories; }
			set { categories = value; }
		}

		public string Description
		{
			get { return description; }
			set { description = value; }
		}
	}
}