using NHibernate;
using NUnit.Framework;
using YourPrjDomain.CompositeId;

namespace uNHAddins.Examples.Course.Tests.Ids
{
	[TestFixture]
	public class CompositeIdFixture : TestCase
	{
		protected override System.Collections.IList Mappings
		{
			get { return new string[] { "CompositeId.Orders.hbm.xml" }; }
		}

		[Test]
		public void CRUD()
		{
			using(ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				Person person = new Person("123456","Tony Manero");

				Order order = new Order();
				order.OrderId = new Order.OrderIdType();
				order.OrderId.OrderNumber = 100;
				order.OrderId.Prefix = "O";
				order.Person = person;

				s.Save(person);
				s.Save(order);
				log.Debug("------ Saved ------");

				t.Commit();
			}

			log.Debug("------ Clean up ------");
			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				Order.OrderIdType orderId = new Order.OrderIdType();
				orderId.OrderNumber = 100;
				orderId.Prefix = "O";
				Order order = s.Get<Order>(orderId);
				s.Delete(order);

				// in this case the Get don't hit the DB
				Person person = s.Get<Person>("123456");
				s.Delete(person);
				t.Commit();
			}
		}
	}
}
