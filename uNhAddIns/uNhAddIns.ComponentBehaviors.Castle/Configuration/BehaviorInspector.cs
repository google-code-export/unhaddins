using System.Linq;
using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.ModelBuilder.Inspectors;
using Castle.MicroKernel.Proxy;

namespace uNhAddIns.ComponentBehaviors.Castle.Configuration
{
	public class BehaviorInspector : MethodMetaInspector
	{
		public override void ProcessModel(IKernel kernel, ComponentModel model)
		{
			if (model.Service is IBehaviorStore)
			{
				foreach (var node in kernel.GraphNodes.OfType<ComponentModel>())
				{
					SetProxyInformation(node, kernel.Resolve<IBehaviorConfigurator>());
				}
				return;
			}

			if (!kernel.HasComponent(typeof (IBehaviorStore))) return;
			var behaviorToProxyResolver = kernel.Resolve<IBehaviorConfigurator>();

			//Get the proxy info
			SetProxyInformation(model, behaviorToProxyResolver);
		}

		static void SetProxyInformation(ComponentModel model, IBehaviorConfigurator behaviorConfigurator)
		{
			var proxyInfo = behaviorConfigurator.GetProxyInformation(model.Implementation);

			//No proxy info.
			if (proxyInfo.AdditionalInterfaces.Count == 0 &&
			    proxyInfo.Interceptors.Count == 0)
				return;

			//apply interceptor references to the model
			foreach (var interceptorType in proxyInfo.Interceptors)
			{
				model.Dependencies.Add(new DependencyModel(DependencyType.Service, null, interceptorType, false));
				model.Interceptors.Add(new InterceptorReference(interceptorType));
			}

			//apply additional interfaces
			ProxyUtil.ObtainProxyOptions(model, true)
				.AddAdditionalInterfaces(proxyInfo.AdditionalInterfaces.ToArray());
		}

		protected override string ObtainNodeName()
		{
			return "no op";
		}
	}
}