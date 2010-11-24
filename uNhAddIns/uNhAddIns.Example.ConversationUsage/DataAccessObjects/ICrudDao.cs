using uNhAddIns.Example.ConversationUsage.Entities;

namespace uNhAddIns.Example.ConversationUsage.DataAccessObjects
{
	public interface ICrudDao<TEntity> : IDao<TEntity> where TEntity : IEntity
	{
		TEntity MakePersistent(TEntity entity);
		void MakeTransient(TEntity entity);
	}
}