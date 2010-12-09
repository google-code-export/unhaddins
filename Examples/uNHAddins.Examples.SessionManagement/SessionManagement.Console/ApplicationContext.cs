using System;
using System.ServiceModel;
using NHibernate;

namespace SessionManagement.WCF.Host
{
	public class ApplicationContext : IExtension<OperationContext>
	{
		#region Readonly & Static Fields

		[ThreadStatic]
		private static ISession session;

		public ApplicationContext(ISession session)
		{
			Session = session;
		}

		#endregion

		#region IExtension<OperationContext> Members
		
		public void Attach(OperationContext owner)
		{
			// no - op
		}

		public void Detach(OperationContext owner)
		{
			// no - op
		}

		#endregion

		#region Class Properties

		public static ApplicationContext Current
		{
			get { return OperationContext.Current.Extensions.Find<ApplicationContext>(); }
		}

		public static ISession Session
		{
			get { return session; }
			set { session = value; }
		}

		#endregion

		public static void CloseSession()
		{
			session.Dispose();
		}
	}
}