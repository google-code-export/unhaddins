using uNhAddIns.Example.AopConversationUsage.Entities;

namespace uNhAddIns.Example.AopConversationUsage.DataAccessObjects
{
	public interface ICrudDao<TEntity> : IDao<TEntity> where TEntity : IEntity
	{
		TEntity MakePersistent(TEntity entity);
		void MakeTransient(TEntity entity);
	}
}