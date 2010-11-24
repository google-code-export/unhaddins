using System.Collections;
using NUnit.Framework;
using SessionManagement.Domain.Model;
using SessionManagement.Infrastructure.InversionOfControl;
using System.Collections.Generic;
using SessionManagement.TestUtils;

namespace SessionManagement.Domain.Tests.Model
{
	[TestFixture]
	public class ProductModelFixture : TestCase
	{
		private IProductModel productModel;

		protected override void OnSetUp()
		{
			productModel = IoC.Resolve<IProductModel>();
		}

		protected override void OnTearDown()
		{
			using (var session = sessions.OpenSession())
			{
				session.Delete("from Product");
				session.Flush();
			}
		}

		[Test]
		public void can_save_product()
		{
			Product product = CreateProduct("A1", "A1 Product", 17.25);
			Product savedProduct = productModel.Save(product);
			
			Assert.That(savedProduct, Is.Not.Null);
			Assert.That(savedProduct.Id > 0);
			productModel.EndConversation();
		}

		[Test]
		public void get_products_returns_emptylist_when_no_products()
		{
			IList<Product> products = productModel.GetProducts();
			
			Assert.That(products, Is.Not.Null, "Get products returned a null list");
			Assert.That(products.Count, Is.EqualTo(0));
			productModel.EndConversation();
		}

		[Test]
		public void get_products_returns_list_with_one_element_when_one_product_exists()
		{
			productModel.Save(CreateProduct("A1", "A1 Product", 17.25));
			
			Assert.That(productModel.GetProducts().Count, Is.EqualTo(1));
			productModel.EndConversation();
		}

		[Test]
		public void get_products_returns_list_with_four_element_when_four_products_exists()
		{
			productModel.Save(CreateProduct("A1", "A1 Product", 17.25));
			productModel.Save(CreateProduct("A2", "A2 Product", 17.25));
			productModel.Save(CreateProduct("A3", "A3 Product", 17.25));
			productModel.Save(CreateProduct("A4", "A4 Product", 17.25));
			
			Assert.That(productModel.GetProducts().Count, Is.EqualTo(4));
			productModel.EndConversation();
		}

		[Test]
		public void product_exists_returns_false_if_no_products()
		{
			Assert.That(productModel.ProductExists(CreateProduct("A1", "A1 Product", 1.0)), Is.False);
			productModel.EndConversation();
		}

		[Test]
		public void product_exists_returns_false_if_some_different_product_exists()
		{
			productModel.Save(CreateProduct("1", "A1 Product", 17.25));
			productModel.Save(CreateProduct("2", "A2 Product", 17.25));
			
			Assert.That(productModel.ProductExists(CreateProduct("A1", "A1 Product", 1.0)), Is.False);
			productModel.EndConversation();
		}

		[Test]
		public void product_exists_returns_true_if_the__product_exists()
		{
			productModel.Save(CreateProduct("A1", "A1 Product", 17.25));
			
			Assert.That(productModel.ProductExists(CreateProduct("A1", "A1 Product", 1.0)), Is.True);
			productModel.EndConversation();
		}

		private Product CreateProduct(string code, string description, double price)
		{
			return new Product { Code = code, Description = description, Price = price };
		}

		#region Overrides of TestCase

		protected override IList Mappings
		{
			get { return new ArrayList(); }
		}

		#endregion
	}
}
