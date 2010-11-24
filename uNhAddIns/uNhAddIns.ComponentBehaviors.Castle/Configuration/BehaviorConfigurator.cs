using System;
using System.Linq;
using uNhAddIns.NHibernateTypeResolver;

namespace uNhAddIns.ComponentBehaviors.Castle.Configuration
{
	public class BehaviorConfigurator : IBehaviorConfigurator
	{
		readonly IBehaviorStore _behaviorStore;


		public BehaviorConfigurator(IBehaviorStore behaviorStore)
		{
			_behaviorStore = behaviorStore;
		}

		#region IBehaviorConfigurator Members

		public ProxyInformation GetProxyInformation(Type implementationType)
		{
			var selectedBehaviors = _behaviorStore.GetBehaviorsForType(implementationType);
			
			//Interfaces that are not already implemented by the implementationType.
            var additionalInterfaces = 
					(from t in selectedBehaviors
			        from attrib in t.GetCustomAttributes(typeof (BehaviorAttribute), true).OfType<BehaviorAttribute>()
			        from i in attrib.AdditionalInterfaces
					where !i.IsAssignableFrom(implementationType)
					select i).ToList();
			
            
			//select interceptors
			var interceptors = selectedBehaviors.ToList();
            
			if (interceptors.Count > 0)
			{
				additionalInterfaces.Add(typeof (IWellKnownProxy));
				interceptors.Insert(0, typeof(GetEntityNameBehavior));
			}

			return new ProxyInformation(implementationType, additionalInterfaces, interceptors);
		}

		#endregion
	}
}