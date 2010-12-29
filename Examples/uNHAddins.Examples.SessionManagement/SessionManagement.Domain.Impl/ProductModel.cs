using System;
using System.Collections.Generic;
using System.ServiceModel;
using SessionManagement.Data.Repositories;
using SessionManagement.Domain.Model;
using uNhAddIns.Adapters;

namespace SessionManagement.Domain.Impl
{
	[PersistenceConversational(MethodsIncludeMode = MethodsIncludeMode.Implicit)]
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
	public class ProductModel : IProductModel
	{
		private readonly IProductRepository productRepository;

		public ProductModel(IProductRepository productRepository)
		{
			this.productRepository = productRepository;
		}

		#region Implementation of IProductManager

		[PersistenceConversation(ConversationEndMode = EndMode.End)]
		public Product Save(Product product)
		{
			if (ProductExists(product))
				throw new Exception("Product exists");

			Console.WriteLine("Saving product Code = {0} Name = {1} Price = {2}", product.Code, product.Description, product.Price);
			return productRepository.MakePersistent(product);
		}

		[PersistenceConversation(ConversationEndMode = EndMode.End)]
		public IList<Product> GetProducts()
		{
			return productRepository.GetAllProducts();
		}
		
		public bool ProductExists(Product product)
		{
			var existingProduct = productRepository.GetProductByCode(product.Code);
			return existingProduct != null;
		}

		[PersistenceConversation(ConversationEndMode = EndMode.Abort)]
		public void EndConversation()
		{
			// Ends the conversation without commiting changes
		}

		#endregion
	}
}
