using System.Collections.Generic;
using NHibernate;
using SessionManagement.Data.Repositories;
using SessionManagement.Domain;

namespace SessionManagement.Data.NH.Repositories
{
	public class ProductRepository : Repository<Product>, IProductRepository
	{
		public ProductRepository(ISessionFactory sessionFactory) : base(sessionFactory) { }

		public IList<Product> GetAllProducts()
		{
			var query = Session.GetNamedQuery("GetAllProductsOrderedByCode");
			return query.List<Product>();
		}

		public Product GetProductByCode(string code)
		{
			var query = Session
				.GetNamedQuery("GetProductByCode")
				.SetParameter("code", code);

			return query.UniqueResult<Product>();
		}
	}
}
