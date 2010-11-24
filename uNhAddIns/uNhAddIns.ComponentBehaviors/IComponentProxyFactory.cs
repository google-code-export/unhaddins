using System;

namespace uNhAddIns.ComponentBehaviors
{
	public interface IComponentProxyFactory
	{
		T GetEntity<T>();
		object GetEntity(Type type);
	}
}