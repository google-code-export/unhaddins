using uNhAddIns.Entities;

namespace ChinookMediaManager.Data
{
	public interface IDaoFactory
	{
		IDaoReadOnly<TEntity> GetDaoReadOnlyOf<TEntity>() where TEntity : Entity;
		IDao<TEntity> GetDaoOf<TEntity>() where TEntity : Entity;
		TDao GetSpecializedDao<TDao, TEntity>() where TDao : class, IDao<TEntity> where TEntity : Entity; 
	}
}