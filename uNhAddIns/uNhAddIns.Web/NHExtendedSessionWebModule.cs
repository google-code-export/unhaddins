using System;
using System.Collections.Generic;
using System.Web;
using log4net;
using NHibernate;
using uNhAddIns.SessionEasier;
using uNhAddIns.SessionEasier.Contexts;

namespace uNhAddIns.Web
{
	public class NHExtendedSessionWebModule : IHttpModule
	{
		public const string EndOfConversationMarkerKey = "EndOfConversationMarker";
		public const string NHibernateSessionKey = "NHibernate.Shared.Sessions";
		private static readonly ILog log = LogManager.GetLogger(typeof (NHExtendedSessionWebModule));

		private ISessionFactoryProvider sfp;

		#region Implementation of IHttpModule

		public void Init(HttpApplication context)
		{
			log.Debug("Obtaining SessionFactoryProvider from Application context.");

			sfp = context.Application[Commons.SessionFactoryProviderKey] as ISessionFactoryProvider;
			if (sfp == null)
			{
				throw new HibernateException("Couldn't obtain SessionFactoryProvider from WebApplicationContext.");
			}
			HttpContext.Current.Items[EndOfConversationMarkerKey] = true;
			context.PreRequestHandlerExecute += Context_PreRequestHandlerExecute;
			context.PostRequestHandlerExecute += Context_PostRequestHandlerExecute;
		}

		public void Dispose() {}

		private void Context_PreRequestHandlerExecute(object sender, EventArgs e)
		{
			var currentSessions = HttpContext.Current.Session[NHibernateSessionKey] as ISession[];
			if (currentSessions != null)
			{
				// Bind the conversation to the context
				foreach (ISession session in currentSessions)
				{
					CurrentSessionContext.Bind(session);
				}
			}
			foreach (ISessionFactory factory in sfp)
			{
				factory.GetCurrentSession().BeginTransaction();
			}
		}

		private void Context_PostRequestHandlerExecute(object sender, EventArgs e)
		{
			// Unbinding Session after processing
			foreach (ISessionFactory factory in sfp)
			{
				CurrentSessionContext.Unbind(factory);
			}

			object endConvParam = HttpContext.Current.Items[EndOfConversationMarkerKey];
			bool endConversation = endConvParam != null ? (bool) endConvParam : false;

			try
			{
				if (endConversation)
				{
					EndConversation();
				}
				else
				{
					ContinueConversation();
				}
			}
			catch (StaleObjectStateException ese)
			{
				ManageStaleObjectState(ese);
			}
			catch (Exception)
			{
				try
				{
					HandleRollbackAfterException();
				}
				finally
				{
					log.Debug("Removing Session from HttpSession");
					HttpContext.Current.Session[NHibernateSessionKey] = null;
				}

				// Let others handle it...
				throw;
			}
		}

		private void HandleRollbackAfterException()
		{
			foreach (ISessionFactory sf in sfp)
			{
				try
				{
					if (sf.GetCurrentSession().Transaction.IsActive)
					{
						log.Debug("Trying to rollback database transaction after exception");
						sf.GetCurrentSession().Transaction.Rollback();
					}
				}
				catch (Exception rbEx)
				{
					log.Error("Could not rollback transaction after exception!", rbEx);
				}
				finally
				{
					log.Error("Cleanup after exception!");
					// Cleanup
					log.Debug("Unbinding Session after exception");
					ISession currentSession = CurrentSessionContext.Unbind(sf);
					log.Debug("Closing Session after exception");
					currentSession.Dispose();
				}
			}
		}

		protected virtual void ManageStaleObjectState(StaleObjectStateException ese)
		{
			throw ese;
		}

		protected void EndConversation()
		{
			foreach (ISessionFactory factory in sfp)
			{
				ISession session = factory.GetCurrentSession();
				session.Flush();
				session.Transaction.Commit();
				session.Close();
			}
			HttpContext.Current.Session[NHibernateSessionKey] = null;
		}

		protected void ContinueConversation()
		{
			var sessions = new List<ISession>();
			foreach (ISessionFactory factory in sfp)
			{
				ISession session = factory.GetCurrentSession();
				session.Transaction.Commit();
				sessions.Add(session);
			}
			HttpContext.Current.Session[NHibernateSessionKey] = sessions.ToArray();
		}

		#endregion
	}
}