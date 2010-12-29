using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ChinookMediaManager.Data;
using Moq;
using uNhAddIns.Entities;

namespace ChinookMediaManager.Domain.Test.Util
{
	public class DaoStub<T> : IDao<T> where T : Entity
	{
		public DaoStub()
		{
			Repository = new List<T>();
			Mock = new Mock<IDao<T>>();
		}

		protected List<T> Repository { get; private set; }

		public DaoStub<T> AddEntity(T entity)
		{
			Repository.Add(entity);
			return this;
		}

		public DaoStub<T> AddEntities(IEnumerable<T> entities)
		{
			Repository.AddRange(entities);
			return this;
		}

		public DaoStub<T> AddEntities(params T[] entities)
		{
			Repository.AddRange(entities);
			return this;
		}

		public static DaoStub<T> New()
		{
			return new DaoStub<T>();
		}

		#region IDao<T> Members

		public T Get(object id)
		{
			return Repository.FirstOrDefault(t => t.Id == (int) id);
		}

		public T GetProxy(object id)
		{
			return Repository.First(t => t.Id == (int) id);
		}

		public void Refresh(T entity)
		{
			Mock.Object.Refresh(entity);
		}

		public virtual T MakePersistent(T entity)
		{
			return Mock.Object.MakePersistent(entity);
		}

		public virtual void MakeTransient(T entity)
		{
			Mock.Object.MakeTransient(entity);
		}

		public virtual IQueryable<T> RetrieveAll()
		{
			return Repository.AsQueryable();
		}

		public IQueryable<T> RetrieveAll(params Expression<Func<T, object>>[] expandProperties)
		{
			return RetrieveAll();
		}

		public IQueryable<T> RetrieveAll(params string[] properties)
		{
			return RetrieveAll();
		}

		public virtual IQueryable<T> Retrieve(Expression<Func<T, bool>> predicate)
		{
			return Repository.Where(predicate.Compile()).AsQueryable();
		}

		public IQueryable<T> Retrieve(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] expandProperties)
		{
			return Retrieve(predicate);
		}

		public IQueryable<T> Retrieve(Expression<Func<T, bool>> predicate, params string[] properties)
		{
			return Repository.Where(predicate.Compile()).AsQueryable();
		}

		public virtual int Count(Expression<Func<T, bool>> predicate)
		{
			return Repository.Count(predicate.Compile());
		}


		public virtual Mock<IDao<T>> Mock { get; private set; }
		#endregion
	}
}