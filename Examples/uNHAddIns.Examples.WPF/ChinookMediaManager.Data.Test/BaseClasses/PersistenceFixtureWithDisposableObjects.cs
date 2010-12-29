using System.Collections;
using NUnit.Framework;

namespace ChinookMediaManager.Data.Test.BaseClasses
{
	public class PersistenceFixtureWithDisposableObjects : PersistenceFixtureBase
	{
		private readonly IList _disposables = new ArrayList();

		public void AddToDisposables(object obj)
		{
			_disposables.Add(obj);
		}

		[TestFixtureTearDown]
		public void DisposeEntities()
		{
			using (var s = Sessions.OpenSession())
			using(var tx = s.BeginTransaction())
			{
				foreach (var obj in _disposables)
				{
					s.Delete(obj);
				}
				tx.Commit();
			}
		}
	}
}