using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.ServiceLocation;
using NHibernate;
using uNhAddIns.SessionEasier;
using uNhAddIns.SessionEasier.Contexts;
using ISession=NHibernate.ISession;

namespace uNhAddIns.WCF
{
	/// <summary>
	/// Manage the NHibernate's session using session-per-call pattern.
	/// </summary>
	/// <remarks>
	/// The session-per-call has idetical behavior of the most famous session-per-request
	/// where, in this case, the NH's session and NH's transaction has the same life-cycle of a call.
	/// </remarks>
	public class NhSessionPerCallContextBehavior : ICallContextInitializer
	{
		private readonly ISessionFactoryProvider sfp;
		public NhSessionPerCallContextBehavior() : this(ServiceLocator.Current.GetInstance<ISessionFactoryProvider>()) {}

		public NhSessionPerCallContextBehavior(ISessionFactoryProvider sessionFactoryProvider)
		{
			sfp = sessionFactoryProvider;
		}

		#region Implementation of ICallContextInitializer

		public object BeforeInvoke(InstanceContext instanceContext, IClientChannel channel, Message message)
		{
			foreach (ISessionFactory sessionFactory in sfp)
			{
				ISession session = sessionFactory.OpenSession();
				CurrentSessionContext.Bind(session);
				session.BeginTransaction();
			}
			return null;
		}

		public void AfterInvoke(object correlationState)
		{
			foreach (ISessionFactory sessionFactory in sfp)
			{
				ISession session = CurrentSessionContext.Unbind(sessionFactory);
				if (!session.IsOpen)
				{
					continue;
				}

				try
				{
					session.Flush();
					if (session.Transaction.IsActive)
					{
						session.Transaction.Commit();
					}
				}
				catch (Exception)
				{
					if (session.Transaction.IsActive)
					{
						session.Transaction.Rollback();
					}
					throw;
				}
				finally
				{
					session.Close();
					session.Dispose();
				}
			}
		}

		#endregion
	}
}