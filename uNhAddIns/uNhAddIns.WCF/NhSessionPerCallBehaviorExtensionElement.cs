using System;
using System.ServiceModel.Configuration;

namespace uNhAddIns.WCF
{
	public class NhSessionPerCallBehaviorExtensionElement : BehaviorExtensionElement
	{
		public override Type BehaviorType
		{
			get { return typeof(NhSessionPerCall); }
		}

		protected override object CreateBehavior()
		{
			return new NhSessionPerCall();
		}
	}
}