namespace uNHAddins.Examples.Course.Tests.Ids
{
	public class AInt32
	{
		private int id;
		private string description;

		public AInt32() {}

		public AInt32(string description)
		{
			this.description = description;
		}

		public virtual int Id
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
