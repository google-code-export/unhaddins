namespace YourPrjDomain.LazyLoad
{
	public class Account
	{
		private Person belongTo;
		private int id;

		public Account()
		{
		}

		public Account(Person belongTo)
		{
			this.belongTo = belongTo;
		}

		public virtual int Id
		{
			get { return id; }
			set { id = value; }
		}

		public virtual Person BelongTo
		{
			get { return belongTo; }
			set { belongTo = value; }
		}
	}
}