using System;

namespace YourPrjDomain.CompositeId
{
	public class Order
	{
		private Person person;
		private OrderIdType orderId;

		[Serializable]
		public class OrderIdType
		{
			private string prefix = "N";
			private int orderNumber;

			public string Prefix
			{
				get { return prefix; }
				set { prefix = value; }
			}

			public int OrderNumber
			{
				get { return orderNumber; }
				set { orderNumber = value; }
			}

			public override bool Equals(object obj)
			{
				OrderIdType that = obj as OrderIdType;
				return that != null && that.Prefix.Equals(Prefix) && that.OrderNumber == OrderNumber;
			}

			public override int GetHashCode()
			{
				return prefix.GetHashCode() ^ orderNumber.GetHashCode();
			}
		}

		public virtual OrderIdType OrderId
		{
			get { return orderId; }
			set { orderId = value; }
		}

		public Person Person
		{
			get { return person; }
			set { person = value; }
		}
	}
}
