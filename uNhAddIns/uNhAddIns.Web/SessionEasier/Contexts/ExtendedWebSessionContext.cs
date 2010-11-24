using NHibernate;
using NHibernate.Engine;
using NHibernate.Transaction;

namespace uNhAddIns.Web.SessionEasier.Contexts
{
	public class ExtendedWebSessionContext : WebSessionContext
	{
		public ExtendedWebSessionContext(ISessionFactoryImplementor factory) : base(factory) {}

		// Always set FlushMode.Never on any Session
		// No automatic flushing of the Session at transaction commit, client calls Flush()
		protected override bool AutoFlushEnabled
		{
			get { return false; }
		}

		// No automatic closing of the Session at transaction commit, client calls Close()
		protected override bool AutoCloseEnabled
		{
			get { return false; }
		}

		protected override ISession BuildOrObtainSession()
		{
			log.Debug("Opening a new Session");
			ISession s = base.BuildOrObtainSession();

			if (s.FlushMode != FlushMode.Never)
			{
				log.Debug("Disabling automatic flushing of the Session");
				s.FlushMode = FlushMode.Never;
			}

			return s;
		}

		// Don't unbind after transaction completion, we expect the client to do this
		// so it can Flush() and Close() if needed (or continue the conversation).
		protected override ISynchronization BuildCleanupSynch()
		{
			return new NoCleanupSynch();
		}

		#region Nested type: NoCleanupSynch

		private class NoCleanupSynch : ISynchronization
		{
			#region ISynchronization Members

			public void BeforeCompletion() {}
			public void AfterCompletion(bool success) {}

			#endregion
		}

		#endregion
	}
}