using System;
using System.Collections.Generic;
using SessionManagement.Domain;
using SessionManagement.Infrastructure.InversionOfControl;
using SessionManagement.Presentation.ViewInterfaces;
using SessionManagement.Domain.Model;

namespace SessionManagement.Presentation.Presenters
{
	public class BrowseProductsPresenter : Presenter<IBrowseProductsView>
	{
		private readonly IProductModel productModel;

		public BrowseProductsPresenter(IBrowseProductsView view) : this(view, IoC.Resolve<IProductModel>())
		{
			
		}

		public BrowseProductsPresenter(IBrowseProductsView view, IProductModel productModel)
			: base(view)
		{
			this.productModel = productModel;
		}

		protected override void ViewInitialized(object sender, EventArgs e)
		{
			var products = productModel.GetProducts();
			View.SetProducts(products);
		}
	}
}
