using uNhAddIns.Example.AopConversationUsage.Entities;

namespace uNhAddIns.Example.AopConversationUsage.DataAccessObjects
{
	public interface IDaoFactory
	{
		ICrudDao<TEntity> GetCrudDaoOf<TEntity>() where TEntity : IEntity;
		TDao GetDao<TDao>();
	}
}