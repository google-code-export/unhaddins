using System;
using SessionManagement.Domain;
using SessionManagement.Infrastructure.InversionOfControl;
using SessionManagement.Presentation.ViewInterfaces;
using SessionManagement.Domain.Model;

namespace SessionManagement.Presentation.Presenters
{
	public class AddProductPresenter : Presenter<IAddProductView>
	{
		private readonly IProductModel productModel;

		public AddProductPresenter(IAddProductView view) : this(view, IoC.Resolve<IProductModel>())
		{
			
		}

		public AddProductPresenter(IAddProductView view, IProductModel productModel)
			: base(view)
		{
			this.productModel = productModel;
			view.AddButtonPressed += view_AddButtonPressed;
			view.CloseView += view_CloseView;
		}

		void view_CloseView(object sender, EventArgs e)
		{
			// If there is an existing conversation end it
			productModel.EndConversation();
		}

		void view_AddButtonPressed(object sender, TEventArgs<Product> e)
		{
			CreateNewProduct(e.Data);
		}

		private void CreateNewProduct(Product product)
		{
			try
			{
				var productExists = productModel.ProductExists(product);

				if (!productExists)
				{
					productModel.Save(product);
					View.ShowMessage("Product added");
					View.Clean();
				}
				else
				{
					View.ShowMessage("Product already exists");
				}
			}
			catch (Exception ex)
			{
				View.ShowMessage(ex.Message);
			}	
		}

		#region Overrides of Presenter<IAddProductView>

		protected override void ViewInitialized(object sender, EventArgs e)
		{
			
		}

		#endregion
	}
}
