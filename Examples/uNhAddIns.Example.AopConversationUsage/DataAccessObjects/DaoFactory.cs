using Microsoft.Practices.ServiceLocation;
using uNhAddIns.Example.AopConversationUsage.Entities;

namespace uNhAddIns.Example.AopConversationUsage.DataAccessObjects
{
	public class DaoFactory : IDaoFactory
	{
		private readonly IServiceLocator serviceLocator;

		public DaoFactory(IServiceLocator serviceLocator)
		{
			this.serviceLocator = serviceLocator;
		}

		#region Implementation of IDaoFactory

		public ICrudDao<TEntity> GetCrudDaoOf<TEntity>() where TEntity : IEntity
		{
			return serviceLocator.GetInstance<ICrudDao<TEntity>>();
		}

		public TDao GetDao<TDao>()
		{
			return serviceLocator.GetInstance<TDao>();
		}

		#endregion
	}
}