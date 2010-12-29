using NHibernate;
using SessionManagement.Data.Repositories;
using SessionManagement.Domain;
using System;

namespace SessionManagement.Data.NH.Repositories
{
	public class OrderRepository : Repository<PurchaseOrder>, IOrderRepository
	{
		public OrderRepository(ISessionFactory sessionFactory) : base(sessionFactory) { }

		public PurchaseOrder GetOrderByNumberAndDate(string number, DateTime dateTime)
		{
			var query = Session
				.GetNamedQuery("GetOrderByNumberAndDate")
				.SetParameter("number", number)
				.SetParameter("dateTime", dateTime);

			return query.UniqueResult<PurchaseOrder>();
		}
	}
}
