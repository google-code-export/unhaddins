using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace uNhAddIns.WCF
{
	public class NhSessionPerCall : Attribute, IServiceBehavior
	{
		#region IServiceBehavior Members

		public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase,
																		 Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters) { }

		public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
		{
			foreach (var cdb in serviceHostBase.ChannelDispatchers)
			{
				var cd = cdb as ChannelDispatcher;
				if (cd != null)
				{
					foreach (EndpointDispatcher ed in cd.Endpoints)
					{
						foreach (DispatchOperation operation in ed.DispatchRuntime.Operations)
						{
							operation.CallContextInitializers.Add(new NhSessionPerCallContextBehavior());
						}
					}
				}
			}
		}

		public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) { }

		#endregion
	}
}