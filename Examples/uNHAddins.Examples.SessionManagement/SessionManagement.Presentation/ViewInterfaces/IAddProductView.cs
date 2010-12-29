using System;
using SessionManagement.Domain;

namespace SessionManagement.Presentation.ViewInterfaces
{
	public interface IAddProductView : IView
	{
		event EventHandler<TEventArgs<Product>> AddButtonPressed;
	}
}
