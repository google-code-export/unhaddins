using uNhAddIns.Example.MultiSessionConversationUsage.Entities;

namespace uNhAddIns.Example.MultiSessionConversationUsage.DataAccessObjects
{
	public interface IDao<TEntity> where TEntity : IEntity
	{
		TEntity Get(int id);
		TEntity GetDelayed(int id);
	}
}