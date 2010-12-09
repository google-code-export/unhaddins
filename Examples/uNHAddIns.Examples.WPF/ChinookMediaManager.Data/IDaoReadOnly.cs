using System;
using System.Linq;
using System.Linq.Expressions;
using uNhAddIns.Entities;

namespace ChinookMediaManager.Data
{
	public interface IDaoReadOnly<T>
		where T : Entity
	{
		T Get(object id);
		T GetProxy(object id);
		void Refresh(T entity);
		IQueryable<T> RetrieveAll();
		IQueryable<T> RetrieveAll(params Expression<Func<T, object>>[] expandProperties);
		IQueryable<T> Retrieve(Expression<Func<T, bool>> predicate);
		IQueryable<T> Retrieve(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] expandProperties);
		int Count(Expression<Func<T, bool>> predicate);
	}
}