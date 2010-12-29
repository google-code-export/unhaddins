using log4net;
using NHibernate.Event;

namespace YourPrjDomain.EventListeners
{
	public class PreSaveEL : IPreInsertEventListener
	{
		private readonly ILog log = LogManager.GetLogger(typeof (PreSaveEL));

		#region IPreInsertEventListener Members

		public bool OnPreInsert(PreInsertEvent @event)
		{
			log.DebugFormat("Entity to save: {0}", @event.Entity.ToString());

			return false;
		}

		#endregion
	}
}