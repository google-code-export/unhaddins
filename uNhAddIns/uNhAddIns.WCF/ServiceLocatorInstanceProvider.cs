using System;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.ServiceLocation;
using System.ServiceModel.Channels;

namespace uNhAddIns.WCF
{
	/// <summary>
	/// Instanciate a service using the Service Locator pattern implemented by
	/// http://www.codeplex.com/CommonServiceLocator
	/// </summary>
	public class ServiceLocatorInstanceProvider : IInstanceProvider
	{
		private readonly Type serviceType;

		public ServiceLocatorInstanceProvider(Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			this.serviceType = serviceType;
		}

		#region Implementation of IInstanceProvider

		public object GetInstance(InstanceContext instanceContext)
		{
			return GetInstance(instanceContext, null);
		}

		public object GetInstance(InstanceContext instanceContext, Message message)
		{
			return ServiceLocator.Current.GetInstance(serviceType);
		}

		public void ReleaseInstance(InstanceContext instanceContext, object instance)
		{
			var disposable = instance as IDisposable;
			if (!ReferenceEquals(null, disposable))
			{
				disposable.Dispose();
			}
		}

		#endregion
	}
}