namespace YourPrjDomain.Core
{
	public class Animal
	{
		public Animal()
		{
		}

		public Animal(string description, float bodyWeight)
		{
			this.description = description;
			this.bodyWeight = bodyWeight;
		}

		private int id;
		public virtual int Id
		{
			get { return id; }
			set { id = value; }
		}

		private string description;
		public virtual string Description
		{
			get { return description; }
			set { description = value; }
		}

		private float bodyWeight;
		public virtual float BodyWeight
		{
			get { return bodyWeight; }
			set { bodyWeight = value; }
		}
	}
}