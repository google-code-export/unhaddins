using uNhAddIns.Example.MultiSessionConversationUsage.Entities;

namespace uNhAddIns.Example.MultiSessionConversationUsage.DataAccessObjects
{
	public interface ICrudDao<TEntity> : IDao<TEntity> where TEntity : IEntity
	{
		TEntity MakePersistent(TEntity entity);
		void MakeTransient(TEntity entity);
	}
}