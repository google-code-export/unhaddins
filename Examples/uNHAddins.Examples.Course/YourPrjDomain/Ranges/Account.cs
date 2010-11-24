namespace YourPrjDomain.Ranges
{
	public class Account
	{
		private int id;
		private DateNullableRange life = new DateNullableRange();

		public DateNullableRange Life
		{
			get { return life; }
			set { life = value; }
		}

		public virtual int Id
		{
			get { return id; }
			set { id = value; }
		}
	}
}
