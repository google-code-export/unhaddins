using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace SessionManagement.WCF.Host
{
	public class SessionEndPointBehavior : IEndpointBehavior
	{
		#region IEndpointBehavior Members

		public void Validate(ServiceEndpoint endpoint)
		{
		}

		public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
		{
		}

		public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
		{
			foreach (var operation in endpointDispatcher.DispatchRuntime.Operations)
			{
				operation.CallContextInitializers.Add(new SessionCallContextInitializer());
			}
		}

		public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
		{
		}

		#endregion
	}
}