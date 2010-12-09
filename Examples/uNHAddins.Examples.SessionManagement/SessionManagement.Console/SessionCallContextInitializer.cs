using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using NHibernate;
using SessionManagement.Infrastructure.InversionOfControl;
using uNhAddIns.SessionEasier;

namespace SessionManagement.WCF.Host
{
	public class SessionCallContextInitializer : ICallContextInitializer
	{
		#region ICallContextInitializer Members

		public object BeforeInvoke(InstanceContext instanceContext, IClientChannel channel, Message message)
		{
			var sfp = IoC.Resolve<ISessionFactoryProvider>();

			foreach (var factoryProvider in sfp)
			{
				var session = factoryProvider.GetCurrentSession();
				session.FlushMode = FlushMode.Commit;
				session.BeginTransaction();
			}

			return null;
		}

		public void AfterInvoke(object correlationState)
		{
			var sfp = IoC.Resolve<ISessionFactoryProvider>();

			foreach (var factory in sfp)
			{
				var session = factory.GetCurrentSession();
				try
				{
					if (session.IsOpen && session.Transaction.IsActive)
					{
						session.Transaction.Commit();
					}
				}
				catch (Exception)
				{
					session.Transaction.Rollback();
					throw;
				}
				finally
				{
					session.Dispose();
				}
			}
		}

		#endregion
	}
}