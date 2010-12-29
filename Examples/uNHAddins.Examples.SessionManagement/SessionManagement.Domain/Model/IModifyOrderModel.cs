using System;

namespace SessionManagement.Domain.Model
{
	public interface IModifyOrderModel
	{
		PurchaseOrder FindOrderOrCreateNew(string number, DateTime dateTime);
		void Persist(PurchaseOrder order);
		void EndConversation();
	}
}
