using uNhAddIns.Example.ConversationUsage.Entities;

namespace uNhAddIns.Example.ConversationUsage.DataAccessObjects
{
	public interface IDao<TEntity> where TEntity: IEntity
	{
		TEntity Get(int id);
		TEntity GetDelayed(int id);
	}
}