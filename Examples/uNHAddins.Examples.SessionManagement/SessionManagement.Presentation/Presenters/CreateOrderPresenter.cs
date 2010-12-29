using System;
using System.Collections.Generic;
using SessionManagement.Domain;
using SessionManagement.Infrastructure.InversionOfControl;
using SessionManagement.Presentation.ViewInterfaces;
using SessionManagement.Domain.Model;

namespace SessionManagement.Presentation.Presenters
{
	public class CreateOrderPresenter : Presenter<ICreateOrderView>
	{
		private readonly IModifyOrderModel modifyOrderModel;
		private PurchaseOrder currentOrder;

		public CreateOrderPresenter(ICreateOrderView view) : this(view, IoC.Resolve<IModifyOrderModel>())
		{
			
		}

		public CreateOrderPresenter(ICreateOrderView view, IModifyOrderModel modifyOrderModel)
			: base(view)
		{
			this.modifyOrderModel = modifyOrderModel;
			view.AddButtonPressed += AddButtonPressed;
			view.SaveButtonPressed += view_SaveButtonPressed;
		}

		void view_SaveButtonPressed(object sender, EventArgs e)
		{
			currentOrder.ClearLines();
			currentOrder.AddLines(View.OrderLines);

			modifyOrderModel.Persist(currentOrder);
		}

		void AddButtonPressed(object sender, EventArgs e)
		{
			// If a conversation was pending then abort it.
			modifyOrderModel.EndConversation();

			currentOrder = modifyOrderModel.FindOrderOrCreateNew(View.OrderNumber, View.OrderDate);

			IList<OrderLine> orderLines = currentOrder.OrderLines;
			
			View.ShowLines(new List<OrderLine>(orderLines));
		}

		#region Overrides of Presenter<ICreateOrderView>

		protected override void ViewInitialized(object sender, EventArgs e)
		{
			// no-op
		}

		#endregion
	}
}
