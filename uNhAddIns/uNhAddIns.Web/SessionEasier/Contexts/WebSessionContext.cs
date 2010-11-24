using System;
using System.Collections.Generic;
using System.Web;
using NHibernate;
using NHibernate.Engine;
using uNhAddIns.SessionEasier;
using uNhAddIns.SessionEasier.Contexts;

namespace uNhAddIns.Web.SessionEasier.Contexts
{
	[Serializable]
	public class WebSessionContext : CurrentSessionContext
	{
		public WebSessionContext(ISessionFactoryImplementor factory) : base(factory) {}

		#region Overrides of AbstractCurrentSessionContext

		protected override IDictionary<ISessionFactory, ISession> GetContextDictionary()
		{
			return HttpContext.Current.Items[Commons.SessionFactoryKey] as IDictionary<ISessionFactory, ISession>;
		}

		protected override void SetContextDictionary(IDictionary<ISessionFactory, ISession> value)
		{
			HttpContext.Current.Items[Commons.SessionFactoryKey] = value;
		}

		#endregion
	}
}