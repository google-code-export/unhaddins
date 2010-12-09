namespace YourPrjDomain.NativeSql
{
	public class Animal
	{
		private float bodyWeight;
		private string description;
		private int id;

		public Animal()
		{
		}

		public Animal(string description, float bodyWeight)
		{
			this.description = description;
			this.bodyWeight = bodyWeight;
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

		public virtual float BodyWeight
		{
			get { return bodyWeight; }
			set { bodyWeight = value; }
		}
	}
}