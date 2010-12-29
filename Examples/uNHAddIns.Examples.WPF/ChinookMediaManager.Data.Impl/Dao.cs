using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Linq;
using uNhAddIns.Entities;

namespace ChinookMediaManager.Data.Impl
{
	public class Dao<T> : IDao<T> where T : Entity
	{
		protected readonly ISessionFactory Factory;

		public Dao(ISessionFactory factory)
		{
			Factory = factory;
		}

		protected ISession CurrentSession
		{
			get { return Factory.GetCurrentSession(); }
		}

		#region IDao<T> Members

		public T Get(object id)
		{
			return CurrentSession.Get<T>(id);
		}

		public T GetProxy(object id)
		{
			return CurrentSession.Load<T>(id);
		}

		public T MakePersistent(T entity)
		{
			CurrentSession.Save(entity);
			return entity;
		}

		public void Refresh(T entity)
		{
			var entityAsEntity = entity as Entity;

			if (entityAsEntity != null && entityAsEntity.Id == 0)
			{
				return;
			}

			object id = CurrentSession.GetIdentifier(entity);
			CurrentSession.Evict(entity);
			CurrentSession.Load(entity, id);
		}

		public IQueryable<T> RetrieveAll()
		{
			return GetLinq();
		}

		public IQueryable<T> RetrieveAll(params Expression<Func<T, object>>[] expandProperties)
		{
			//return GetLinq().Fetch();
			return GetLinq();
		}

		public virtual IQueryable<T> Retrieve(Expression<Func<T, bool>> predicate)
		{
			return GetLinq().Where(predicate);
		}

		public IQueryable<T> Retrieve(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] expandProperties)
		{
			//return GetLinq().Fetch();
			return Retrieve(predicate);
		}

		public int Count(Expression<Func<T, bool>> predicate)
		{
			return GetLinq().Where(predicate).Count();
		}

		public void MakeTransient(T entity)
		{
			CurrentSession.Delete(entity);
		}

		#endregion

		private IQueryable<T> GetLinq()
		{
			return CurrentSession.Query<T>();
		}
	}
}