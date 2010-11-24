using System;

namespace uNhAddIns.ComponentBehaviors
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class BehaviorAttribute : Attribute
	{
		public int Order { get; private set; }
		public Type[] AdditionalInterfaces { get; private set; }

		public BehaviorAttribute(int order, params Type[] additionalInterfaces)
		{
			Order = order;
			AdditionalInterfaces = additionalInterfaces;
		}
	}
}