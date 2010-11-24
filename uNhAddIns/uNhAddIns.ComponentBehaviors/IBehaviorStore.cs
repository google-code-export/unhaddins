using System;
using System.Collections.Generic;

namespace uNhAddIns.ComponentBehaviors
{
	public interface IBehaviorStore
	{
		ICollection<Type> GetBehaviorsForType(Type type);
	}
}