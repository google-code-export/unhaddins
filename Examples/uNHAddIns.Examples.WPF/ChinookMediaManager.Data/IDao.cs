using uNhAddIns.Entities;

namespace ChinookMediaManager.Data
{
	public interface IDao<T> : IDaoReadOnly<T>
		where T : Entity
	{
		T MakePersistent(T entity);
		void MakeTransient(T entity);
	}
}