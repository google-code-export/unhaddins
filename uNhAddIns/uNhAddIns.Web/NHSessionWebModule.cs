using System;
using System.Web;
using System.Web.Handlers;
using log4net;
using NHibernate;
using uNhAddIns.SessionEasier;

namespace uNhAddIns.Web
{
	public class NHSessionWebModule : IHttpModule
	{
		private static readonly ILog log = LogManager.GetLogger(typeof (NHSessionWebModule));
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

			context.BeginRequest += ApplicationBeginRequest;
			context.EndRequest += ApplicationEndRequest;
		}

		public void Dispose() {}

		private void ApplicationBeginRequest(object sender, EventArgs e)
		{
			if (!RequestMayNeedIterationWithPersistence(sender as HttpApplication))
			{
				return;
			}
			foreach (ISessionFactory factory in sfp)
			{
				factory.GetCurrentSession().BeginTransaction();
			}
		}

		private void ApplicationEndRequest(object sender, EventArgs e)
		{
			if (!RequestMayNeedIterationWithPersistence(sender as HttpApplication))
			{
				return;
			}
			foreach (ISessionFactory factory in sfp)
			{
				ISession session = factory.GetCurrentSession();
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

		private static bool RequestMayNeedIterationWithPersistence(HttpApplication application)
		{
			if (application == null)
			{
				return false;
			}

			HttpContext context = application.Context;
			return context != null && context.Handler != null &&
				!(context.Handler is AssemblyResourceLoader) && !(context.Handler is DefaultHttpHandler);
		}

		#endregion
	}
}