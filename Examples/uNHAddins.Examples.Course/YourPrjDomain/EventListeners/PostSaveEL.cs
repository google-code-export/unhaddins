using log4net;
using NHibernate.Event;

namespace YourPrjDomain.EventListeners
{
	public class PostSaveEL : IPostInsertEventListener
	{
		private readonly ILog log = LogManager.GetLogger(typeof (PostSaveEL));

		#region IPostInsertEventListener Members

		public void OnPostInsert(PostInsertEvent @event)
		{
			log.Debug("This is a post-save at the entity");
		}

		#endregion
	}
}