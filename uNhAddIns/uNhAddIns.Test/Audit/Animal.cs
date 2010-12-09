using System;

namespace uNhAddIns.Test.Audit
{
	public class Animal
	{
		public virtual string Description { get; set; }
	}

	public class Reptile : Animal
	{
		public virtual double BodyTemperature { get; set; }
	}

	public class Lizard : Reptile {}

	public class Mammal : Animal
	{
		public virtual bool Pregnant { get; set; }
		public virtual DateTime Birthdate { get; set; }
	}

	public class DomesticAnimal : Animal
	{
		public virtual string Name { get; set; }
		public virtual Person Owner { get; set; }
	}

	public class Cat : Animal {}

	public class Dog : Animal {}
}