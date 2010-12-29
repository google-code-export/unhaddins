using uNhAddIns.Example.AopConversationUsage.Entities;

namespace uNhAddIns.Example.AopConversationUsage.DataAccessObjects
{
	public interface IDao<TEntity> where TEntity : IEntity
	{
		TEntity Get(int id);
		TEntity GetDelayed(int id);
	}
}