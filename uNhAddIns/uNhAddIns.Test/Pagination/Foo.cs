namespace uNhAddIns.Test.Pagination
{
	public class Foo 
	{
		public Foo() {}
		public Foo(string name, string description)
		{
			Name = name;
			Description = description;
		}

		public virtual string Name { get; set; }
		public virtual string Description { get; set; }
	}
}