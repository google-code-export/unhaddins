using NHibernate.Cfg;
using NHibernate.Event;

namespace uNhAddIns.Audit
{
	public interface IAuditor
	{
		void Initialize(Configuration cfg);
		void Inserting(PreInsertEvent eventArgs);
		void Updating(PreUpdateEvent eventArgs);
		void Deleting(PreDeleteEvent eventArgs);
		void Inserted(PostInsertEvent eventArgs);
		void Updated(PostUpdateEvent eventArgs);
		void Deleted(PostDeleteEvent eventArgs);
	}
}