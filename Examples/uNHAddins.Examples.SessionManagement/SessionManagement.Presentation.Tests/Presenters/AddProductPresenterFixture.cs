using NUnit.Framework;
using SessionManagement.Domain;
using SessionManagement.Domain.Model;
using SessionManagement.Presentation.Presenters;
using SessionManagement.Presentation.ViewInterfaces;
using Rhino.Mocks;

namespace SessionManagement.Presentation.Tests.Presenters
{
	[TestFixture]
	public class AddProductPresenterFixture
	{
		private IProductModel mockProductModel;
		private IAddProductView mockAddProductView;
		private AddProductPresenter addProductPresenter;
		
		[SetUp]
		public void SetUp()
		{
			mockAddProductView = MockRepository.GenerateMock<IAddProductView>();
			mockProductModel = MockRepository.GenerateMock<IProductModel>();
			addProductPresenter = new AddProductPresenter(mockAddProductView, mockProductModel);
		}

		[Test]
		public void model_save_method_is_called_for_a_new_product()
		{
			var product = new Product { Code = "A1", Description = "A1 Desc", Price = 1.0 };

			mockProductModel.Expect(p => p.Save(product)).IgnoreArguments();

			RaiseAddButtonPressed(product);

			mockProductModel.VerifyAllExpectations();
		}

		[Test]
		public void model_save_method_is_called_if_product_does_not_exist()
		{
			var product = new Product { Code = "A1", Description = "A1 Desc", Price = 1.0 };
			mockProductModel.Stub(p => p.ProductExists(product)).IgnoreArguments().Return(false);
			
			RaiseAddButtonPressed(product);

			mockProductModel.AssertWasCalled(m => m.Save(product));
		}

		[Test]
		public void model_save_method_is_not_called_if_product_exists()
		{
			var product = new Product { Code = "A1", Description = "A1 Desc", Price = 1.0 };
			mockProductModel.Stub(p => p.ProductExists(product)).IgnoreArguments().Return(true);
			
			RaiseAddButtonPressed(product);

			mockProductModel.AssertWasNotCalled(m => m.Save(product));
		}

		private void RaiseAddButtonPressed(Product product)
		{
			mockAddProductView
				.GetEventRaiser(v => v.AddButtonPressed += null)
				.Raise(null, new TEventArgs<Product>(product));
		}

		[Test]
		public void view_message_is_shown_after_save()
		{
			mockAddProductView.Expect(v => v.ShowMessage(null)).IgnoreArguments();
			RaiseAddButtonPressed(new Product());
			mockAddProductView.VerifyAllExpectations();
		}

		[Test]
		public void view_is_cleaned_after_save()
		{
			mockAddProductView.Expect(v => v.Clean());
			RaiseAddButtonPressed(new Product());
			mockAddProductView.VerifyAllExpectations();
		}
	}
}