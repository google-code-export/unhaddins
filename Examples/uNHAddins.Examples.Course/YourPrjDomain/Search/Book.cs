namespace YourPrjDomain.Search
{
	using NHibernate.Search.Attributes;

	[Indexed]
	public class Book
	{
		private string author;
		private int id;
		private string name;
		private string summary;

		public Book()
		{
		}

		public Book(string name, string author, string summary)
		{
			this.author = author;
			this.name = name;
			this.summary = summary;
		}

		[DocumentId]
		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		[Field(Index.Tokenized, Store = Store.Yes)]
		public string Author
		{
			get { return author; }
			set { author = value; }
		}

		[Field(Index.Tokenized, Store = Store.Yes)]
		public string Summary
		{
			get { return summary; }
			set { summary = value; }
		}

		[Field(Index.Tokenized, Store = Store.Yes)]
		public string Name
		{
			get { return name; }
			set { name = value; }
		}
	}
}
