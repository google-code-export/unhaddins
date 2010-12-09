using System;
using System.IO;
using System.Web;
using log4net;
using NHibernate;
using uNhAddIns.SessionEasier;

namespace uNhAddIns.Web
{
	public class NHSessionWebModule : IHttpModule
	{
		private static readonly ILog log = LogManager.GetLogger(typeof (NHSessionWebModule));
		private static readonly string[] NoPersistenceFileExtensions = new string[] { ".jpg", ".gif", ".png", ".css", ".js", ".swf", ".xap" };
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
			if (context == null)
			{
				return false;
			}
			string fileExtension = Path.GetExtension(context.Request.PhysicalPath);
			return fileExtension != null && Array.IndexOf(NoPersistenceFileExtensions, fileExtension.ToLower()) < 0;
		}

		#endregion
	}
}