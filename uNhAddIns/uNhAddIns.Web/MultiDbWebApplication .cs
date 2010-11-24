using System;
using System.Web;
using uNhAddIns.SessionEasier;

namespace uNhAddIns.Web
{
	public class MultiDbWebApplication : HttpApplication
	{
		public virtual void Application_Start(object sender, EventArgs e)
		{
			Application[Commons.SessionFactoryProviderKey] = new MultiSessionFactoryProvider();
		}

		public virtual void Application_End(object sender, EventArgs e)
		{
			if (Application[Commons.SessionFactoryProviderKey] != null)
			{
				var sfp = Application[Commons.SessionFactoryProviderKey] as ISessionFactoryProvider;
				if (sfp != null)
				{
					sfp.Dispose();
					Application.Remove(Commons.SessionFactoryProviderKey);
				}
			}
		}
	}
}