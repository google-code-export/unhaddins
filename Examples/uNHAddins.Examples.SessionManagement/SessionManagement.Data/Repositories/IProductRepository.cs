using System.Collections.Generic;
using SessionManagement.Domain;

namespace SessionManagement.Data.Repositories
{
	public interface IProductRepository : IRepository<Product>
	{
		IList<Product> GetAllProducts();
		Product GetProductByCode(string code);
	}
}