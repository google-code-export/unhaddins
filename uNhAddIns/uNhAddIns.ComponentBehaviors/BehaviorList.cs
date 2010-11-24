using System;
using System.Collections.Generic;

namespace uNhAddIns.ComponentBehaviors
{
	public class BehaviorList
	{
		private readonly ICollection<Type> _behaviors = new HashSet<Type>();	

		/// <summary>
		/// Add a new behavior for a component
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public BehaviorList Add<T> ()
		{
			_behaviors.Add(typeof(T));
			return this;
		}

		public ICollection<Type> GetBehaviors()
		{
			return _behaviors;
		}
	}
}