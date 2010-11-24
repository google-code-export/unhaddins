using NHibernate;
using uNhAddIns.Example.ConversationUsage.Entities;

namespace uNhAddIns.Example.ConversationUsage.DataAccessObjects
{
	public class BaseCrudDao<TEntity> : ICrudDao<TEntity> where TEntity : IEntity
	{
		protected readonly ISessionFactory factory;

		protected BaseCrudDao(ISessionFactory factory)
		{
			this.factory = factory;
		}

		#region ICrudDao<TEntity> Members

		public TEntity MakePersistent(TEntity entity)
		{
			factory.GetCurrentSession().SaveOrUpdate(entity);
			return entity;
		}

		public void MakeTransient(TEntity entity)
		{
			factory.GetCurrentSession().Delete(entity);
		}

		public TEntity Get(int id)
		{
			return factory.GetCurrentSession().Get<TEntity>(id);
		}

		public TEntity GetDelayed(int id)
		{
			return factory.GetCurrentSession().Load<TEntity>(id);
		}

		#endregion
	}
}