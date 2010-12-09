using System;
using System.ServiceModel.Configuration;
namespace uNhAddIns.WCF
{
	public class ServiceLocatorBehaviorExtensionElement : BehaviorExtensionElement
	{
		#region Overrides of BehaviorExtensionElement

		protected override object CreateBehavior()
		{
			return new InstanciateThroughServiceLocator();
		}

		public override Type BehaviorType
		{
			get { return typeof(InstanciateThroughServiceLocator); }
		}

		#endregion
	}
}