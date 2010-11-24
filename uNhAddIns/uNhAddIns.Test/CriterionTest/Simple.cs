namespace uNhAddIns.Test.CriterionTest
{
	public class Simple
	{
#pragma warning disable 649
		private int id;
#pragma warning restore 649
		public virtual int Id
		{
			get { return id; }
		}

		private string name;
		public virtual string Name
		{
			get { return name; }
			set { name = value; }
		}

		private int number;
		public virtual int Number
		{
			get { return number; }
			set { number = value; }
		}
	}
}