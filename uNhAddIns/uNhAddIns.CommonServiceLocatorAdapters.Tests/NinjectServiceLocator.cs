using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using Ninject;

namespace uNhAddIns.NinjectAdapters.Tests
{
	/// <summary>
	/// Translates calls from IServiceLocator to Spring.NET's IListableObjectFactory-based containers
	/// </summary>
	internal class NinjectServiceLocator : ServiceLocatorImplBase
	{
		// holds the underlying factory
		private readonly IKernel kernel;

		/// <summary>
		/// Creates a new adapter instance, using the specified <paramref name="kernel"/>
		/// for resolving service instance requests.
		/// </summary>
		/// <param name="kernel">the actual factory used for resolving service instance requests.</param>
		public NinjectServiceLocator(IKernel kernel)
		{
			if (kernel == null)
			{
				throw new ArgumentNullException("kernel", "A factory is mandatory");
			}
			this.kernel = kernel;
		}

		/// <summary>
		/// Resolves a requested service instance.
		/// </summary>
		/// <param name="serviceType">Type of instance requested.</param>
		/// <param name="key">Name of registered service you want. May be null.</param>
		/// <returns>
		/// The requested service instance or null, if <paramref name="key"/> is not found.
		/// </returns>
		protected override object DoGetInstance(Type serviceType, string key)
		{
			return kernel.Get(serviceType);
		}

		/// <summary>
		/// Resolves service instances by type.
		/// </summary>
		/// <param name="serviceType">Type of service requested.</param>
		/// <returns>
		/// Sequence of service instance objects matching the <paramref name="serviceType"/>.
		/// </returns>
		protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
		{
			return kernel.GetAll(serviceType);
		}
	}
}
