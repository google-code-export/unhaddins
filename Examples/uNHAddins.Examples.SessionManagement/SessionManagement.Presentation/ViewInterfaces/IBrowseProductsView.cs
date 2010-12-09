using System;
using System.Collections.Generic;
using SessionManagement.Domain;
namespace SessionManagement.Presentation.ViewInterfaces
{
	public interface IBrowseProductsView : IView
	{
		Product SelectedProduct { get; }
		void SetProducts(IList<Product> products);
		event EventHandler<TEventArgs<Product>> ProductSelected;
	}
}
