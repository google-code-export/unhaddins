using uNhAddIns.Example.ConversationUsage.Entities;
namespace uNhAddIns.Example.ConversationUsage.DataAccessObjects
{
	public interface IDaoFactory
	{
		ICrudDao<TEntity> GetCrudDaoOf<TEntity>() where TEntity : IEntity;
		TDao GetDao<TDao>();
	}
}