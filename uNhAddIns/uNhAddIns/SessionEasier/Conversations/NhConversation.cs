using System;
using System.Collections.Generic;
using log4net;
using NHibernate;

namespace uNhAddIns.SessionEasier.Conversations
{
	[Serializable]
	public class NhConversation : AbstractConversation
	{
		// TODO : wrap de session
		// TODO: manage transaction

		private const string sessionsContextKey = "uNhAddIns.Conversations.NHSessions";
		[NonSerialized] protected static readonly ILog log = LogManager.GetLogger(typeof (NhConversation));
		[NonSerialized] private readonly ISessionFactoryProvider factoriesProvider;

		public NhConversation(ISessionFactoryProvider factoriesProvider)
		{
			if (factoriesProvider == null)
			{
				throw new ArgumentNullException("factoriesProvider");
			}
			this.factoriesProvider = factoriesProvider;
		}

		public NhConversation(ISessionFactoryProvider factoriesProvider, string id) : base(id)
		{
			if (factoriesProvider == null)
			{
				throw new ArgumentNullException("factoriesProvider");
			}
			this.factoriesProvider = factoriesProvider;
		}

		#region Overrides of AbstractConversation

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				IDictionary<ISessionFactory, ISession> sessions = GetFromContext();
				foreach (var pair in sessions)
				{
					pair.Value.Dispose();
				}
				sessions.Clear();
			}
		}

		protected override void DoStart()
		{
			DoResume();
		}

		protected override void DoPause()
		{
			IDictionary<ISessionFactory, ISession> sessions = GetFromContext();
			foreach (var pair in sessions)
			{
				pair.Value.Disconnect();
			}
		}

		protected override void DoResume()
		{
			IDictionary<ISessionFactory, ISession> sessions = GetFromContext();
			if (sessions.Count > 0)
			{
				var factoriesToRebind = new List<ISessionFactory>(5);
				foreach (var pair in sessions)
				{
					ISession s = pair.Value;
					if (s != null && s.IsOpen)
					{
						if (!s.IsConnected)
						{
							pair.Value.Reconnect();
						}
					}
					else
					{
						factoriesToRebind.Add(pair.Key);
					}
				}
				foreach (var factory in factoriesToRebind)
				{
					Bind(factory.OpenSession());					
				}
			}
			else
			{
				// Bind a session for each SessionFactory
				foreach (ISessionFactory sessionFactory in factoriesProvider)
				{
					Bind(sessionFactory.OpenSession());
				}
			}
		}

		protected override void DoEnd()
		{
			IDictionary<ISessionFactory, ISession> sessions = GetFromContext();
			foreach (var pair in sessions)
			{
				ISession session = pair.Value;
				if (session != null && session.IsOpen)
				{
					session.Close();
				}
			}
		}

		#endregion

		protected IDictionary<ISessionFactory, ISession> GetFromContext()
		{
			object result;
			if (!Context.TryGetValue(sessionsContextKey, out result))
			{
				result = new Dictionary<ISessionFactory, ISession>(2);
				Context[sessionsContextKey] = result;
			}
			return (IDictionary<ISessionFactory, ISession>) result;
		}

		public virtual ISession GetSession(ISessionFactory sessionFactory)
		{
			IDictionary<ISessionFactory, ISession> sessions = GetFromContext();
			ISession result;
			if (!sessions.TryGetValue(sessionFactory, out result))
			{
				throw new ConversationException("The conversation was not started or was disposed or the SessionFactoryProvider don't manage it.");
			}
			return result;
		}

		public void Bind(ISession session)
		{
			ISessionFactory factory = session.SessionFactory;
			CleanupAnyOrphanedSession(factory);
			DoBind(session, factory);
		}

		private void CleanupAnyOrphanedSession(ISessionFactory factory)
		{
			ISession orphan = DoUnbind(factory);
			if (orphan != null && orphan.IsOpen)
			{
				log.Warn("Already session bound on call to Bind(); make sure you clean up your sessions!");
				try
				{
					if (orphan.Transaction != null && orphan.Transaction.IsActive)
					{
						try
						{
							orphan.Transaction.Rollback();
						}
						catch (Exception t)
						{
							log.Debug("Unable to rollback transaction for orphaned session", t);
						}
					}
					orphan.Close();
				}
				catch (Exception t)
				{
					log.Debug("Unable to close orphaned session", t);
				}
			}
		}

		private ISession DoUnbind(ISessionFactory factory)
		{
			IDictionary<ISessionFactory, ISession> sessionDic = GetFromContext();
			ISession session = null;
			if (sessionDic != null)
			{
				sessionDic.TryGetValue(factory, out session);
				sessionDic[factory] = null;
			}
			return session;
		}

		public void Unbind(ISession session)
		{
			DoUnbind(session.SessionFactory);
		}

		private void DoBind(ISession session, ISessionFactory factory)
		{
			IDictionary<ISessionFactory, ISession> sessions = GetFromContext();
			sessions[factory] = session;
		}
	}
}