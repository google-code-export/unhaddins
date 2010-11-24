using System;
using System.Collections.Generic;

namespace uNhAddIns.ComponentBehaviors.Castle.Configuration
{
	public class ProxyInformation
	{
		public ProxyInformation(Type entityType,
		                        ICollection<Type> additionalInterfaces,
		                        ICollection<Type> interceptorReferences)
		{
			EntityType = entityType;
			AdditionalInterfaces = additionalInterfaces;
			Interceptors = interceptorReferences;
		}

		public Type EntityType { get; private set; }
		public ICollection<Type> AdditionalInterfaces { get; private set; }
		public ICollection<Type> Interceptors { get; private set; }
	}
}