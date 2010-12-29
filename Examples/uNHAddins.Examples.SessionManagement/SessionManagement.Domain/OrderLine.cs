using System;

namespace SessionManagement.Domain
{
	public class OrderLine : BaseEntity
	{
		public virtual string LineNumber { get; set; }
		public virtual Product Product { get; set; }
		public virtual int Quantity { get; set; }
		public virtual double UnitPrice { get; set; }
		public virtual PurchaseOrder Order { get; set; }

		public virtual string ProductCode
		{
			get { return Product == null ? string.Empty : Product.Code; }
		}
	}
}