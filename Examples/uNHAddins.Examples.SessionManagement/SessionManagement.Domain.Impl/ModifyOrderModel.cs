using System;
using SessionManagement.Data.Repositories;
using SessionManagement.Domain.Model;
using uNhAddIns.Adapters;

namespace SessionManagement.Domain.Impl
{
	[PersistenceConversational(MethodsIncludeMode = MethodsIncludeMode.Implicit)]
	public class ModifyOrderModel : IModifyOrderModel
	{
		private readonly IOrderRepository orderRepository;

		public ModifyOrderModel(IOrderRepository orderRepository)
		{
			this.orderRepository = orderRepository;
		}

		#region Implementation of IProductManager

		[PersistenceConversation(ConversationEndMode = EndMode.Abort)]
		public void EndConversation()
		{
			// Ends the conversation without commiting pending changes
		}

		public PurchaseOrder FindOrderOrCreateNew(string number, DateTime dateTime)
		{
			var order = orderRepository.GetOrderByNumberAndDate(number, dateTime.Date);

			if (order == null)
			{
				order = new PurchaseOrder { Date = dateTime, Number = number };
			}
			
			return order;
		}

		[PersistenceConversation(ConversationEndMode = EndMode.End)]
		public void Persist(PurchaseOrder order)
		{
			orderRepository.MakePersistent(order);
		}

		#endregion
	}
}
