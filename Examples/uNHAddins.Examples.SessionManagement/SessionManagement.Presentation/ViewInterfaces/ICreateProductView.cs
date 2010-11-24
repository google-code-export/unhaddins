using System;
using System.Collections.Generic;
using SessionManagement.Domain;

namespace SessionManagement.Presentation.ViewInterfaces
{
	public interface ICreateOrderView : IView
	{
		event EventHandler AddButtonPressed;
		event EventHandler SaveButtonPressed;
		DateTime OrderDate { get; }
		string OrderNumber { get; }
		IList<OrderLine> OrderLines { get; }
		void ShowLines(IList<OrderLine> lines);
	}
}
