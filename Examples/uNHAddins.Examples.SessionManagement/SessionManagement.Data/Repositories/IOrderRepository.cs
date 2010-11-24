using System;
using SessionManagement.Domain;

namespace SessionManagement.Data.Repositories
{
	public interface IOrderRepository : IRepository<PurchaseOrder>
	{
		PurchaseOrder GetOrderByNumberAndDate(string number, DateTime dateTime);
	}
}