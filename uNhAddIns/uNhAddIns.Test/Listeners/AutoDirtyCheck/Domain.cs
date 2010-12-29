using System;
using Iesi.Collections.Generic;

namespace uNhAddIns.Test.Listeners.AutoDirtyCheck
{
	public abstract class Entity
	{
		public virtual int Id { get; private set; }
	}

	public abstract class Animal: Entity
	{
		public virtual string Description { get; set; }
	}

	public class Reptile: Animal
	{
		public virtual float BodyTemperature { get; set; }
	}

	public class Human : Animal
	{
		public virtual string Name { get; set; }
		public virtual string NickName { get; set; }
		public virtual DateTime Birthdate { get; set; }
	}

	public class Family<T>: Entity where T: Animal
	{
		public virtual T Father { get; set; }
		public virtual T Mother { get; set; }
		public virtual ISet<T> Children { get; set; }
	}
}