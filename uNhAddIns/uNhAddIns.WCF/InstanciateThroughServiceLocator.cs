using System;
using System.Collections.ObjectModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace uNhAddIns.WCF
{
	public class InstanciateThroughServiceLocator: Attribute, IServiceBehavior
	{
		#region Implementation of IServiceBehavior

		public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) { }

		public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase,
																		 Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters) { }

		public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
		{
			foreach (var cdb in serviceHostBase.ChannelDispatchers)
			{
				var cd = cdb as ChannelDispatcher;
				if (cd != null)
				{
					foreach (var ed in cd.Endpoints)
					{
						ed.DispatchRuntime.InstanceProvider = new ServiceLocatorInstanceProvider(serviceDescription.ServiceType);
					}
				}
			}
		}

		#endregion
	}
		
	
}