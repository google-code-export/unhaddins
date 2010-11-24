using System.Collections.Generic;
using System.ServiceModel;

namespace SessionManagement.Domain.Model
{
	[ServiceContract]
	public interface IProductModel
	{
		[OperationContract]
		Product Save(Product product);

		[OperationContract]
		IList<Product> GetProducts();

		[OperationContract]
		bool ProductExists(Product product);

		void EndConversation();
	}
}
