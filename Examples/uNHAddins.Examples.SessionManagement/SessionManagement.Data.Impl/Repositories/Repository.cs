using NHibernate;
using SessionManagement.Data.Repositories;

namespace SessionManagement.Data.NH.Repositories
{
	public class Repository<T> : IRepository<T>
	{
		private readonly ISessionFactory factory;

		public Repository(ISessionFactory factory)
		{
			this.factory = factory;
		}

		#region Implementation of IRepository<T>

		public T Get(object id)
		{
			return Session.Get<T>(id);
		}

		public T Load(object id)
		{
			return Session.Load<T>(id);
		}

		public T MakePersistent(T entity)
		{
			Session.SaveOrUpdate(entity);
			return entity;
		}

		public void MakeTransient(T entity)
		{
			Session.Delete(entity);
		}

		protected ISession Session
		{
			get { return factory.GetCurrentSession(); }
		}

		#endregion
	}
}
