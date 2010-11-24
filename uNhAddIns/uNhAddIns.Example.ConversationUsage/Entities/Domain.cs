using System;
using Iesi.Collections.Generic;

namespace uNhAddIns.Example.ConversationUsage.Entities
{
	public abstract class Animal : BaseEntity
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

	public class Family<T> : BaseEntity where T : Animal
	{
		public virtual T Father { get; set; }
		public virtual T Mother { get; set; }
		public virtual ISet<T> Childs { get; set; }
	}
}