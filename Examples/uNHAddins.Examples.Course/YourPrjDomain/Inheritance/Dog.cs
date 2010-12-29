namespace YourPrjDomain.Inheritance
{
	public class Dog : Animal
	{
		public Dog()
		{
		}

		public Dog(string name, string breed)
		{
			Name = name;
			this.breed = breed;
		}

		private string breed;

		public string Breed
		{
			get { return breed; }
			set { breed = value; }
		}
	}
}