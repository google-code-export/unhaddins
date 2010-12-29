namespace uNHAddins.Examples.Course.Tests.Ids
{
	public class AInt64
	{
		private long id;
		private string description;

		public AInt64() { }

		public AInt64(string description)
		{
			this.description = description;
		}

		public virtual long Id
		{
			get { return id; }
			set { id = value; }
		}

		public virtual string Description
		{
			get { return description; }
			set { description = value; }
		}

		public override string ToString()
		{
			return string.Format("#{0}({1})", id, description);
		}
	}
}
