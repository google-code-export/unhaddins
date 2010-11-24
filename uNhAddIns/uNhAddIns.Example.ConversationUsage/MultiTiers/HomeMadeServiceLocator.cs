using System;
using System.Collections.Generic;

namespace uNhAddIns.Example.ConversationUsage.MultiTiers
{
	public class HomeMadeServiceLocator : IServiceLocator
	{
		private readonly Dictionary<string, object> services = new Dictionary<string, object>();
		private readonly Dictionary<string, object> transientServices = new Dictionary<string, object>();

		#region IServiceLocator Members

		public T Resolve<T>(string key)
		{
			object service;
			if (services.TryGetValue(key, out service))
			{
				return (T) service;
			}
			else if (transientServices.TryGetValue(key, out service))
			{
				var constructorDelegate = (Func<T>) service;
				return constructorDelegate();
			}
			return default(T);
		}

		public T Resolve<T>()
		{
			return Resolve<T>(typeof (T).AssemblyQualifiedName);
		}

		#endregion

		public void LoadSingletonService(string key, object service)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}
			if (service == null)
			{
				throw new ArgumentNullException("service");
			}
			services[key] = service;
		}

		public void LoadSingletonService<T>(T service)
		{
			LoadSingletonService(typeof (T).AssemblyQualifiedName, service);
		}

		public void LoadDelegatedService<T>(string key, Func<T> constructorDelegate)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}
			if (constructorDelegate == null)
			{
				throw new ArgumentNullException("constructorDelegate");
			}
			transientServices[typeof (T).AssemblyQualifiedName] = constructorDelegate;
		}

		public void LoadDelegatedService<T>(Func<T> constructorDelegate)
		{
			LoadDelegatedService(typeof (T).AssemblyQualifiedName, constructorDelegate);
		}
	}
}