namespace YourPrjDomain.Inheritance
{
	public class Frog : Animal
	{
		public Frog()
		{
		}

		public Frog(string name, int tongueLength)
		{
			Name = name;
			this.tongueLength = tongueLength;
		}

		private int tongueLength;

		public int TongueLength
		{
			get { return tongueLength; }
			set { tongueLength = value; }
		}
	}
}