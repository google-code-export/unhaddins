using System;
using System.Collections;
using NUnit.Framework;
using SessionManagement.Domain.Impl;
using SessionManagement.Domain.Model;
using SessionManagement.Infrastructure.InversionOfControl;
using SessionManagement.TestUtils;
using uNhAddIns.TestUtils.Logging;

namespace SessionManagement.Domain.Tests.Model
{
	[TestFixture]
	public class OrderModelFixture : TestCase
	{
		private IModifyOrderModel modifyOrderModel;

		protected override void OnSetUp()
		{
			modifyOrderModel = IoC.Resolve<IModifyOrderModel>();
		}

		protected override void OnTearDown()
		{
			using (var session = sessions.OpenSession())
			{
				session.Delete("from PurchaseOrder");
				session.Flush();
			}
		}

		[Test]
		public void can_save_order()
		{
			var purchaseOrder = new PurchaseOrder
			                    	{
			                    		Date = DateTime.Now,
			                    		Number = "N1"
			                    	};
			modifyOrderModel.Persist(purchaseOrder);
			Assert.That(purchaseOrder, Is.Not.Null);
			Assert.That(purchaseOrder.Id > 0);
		}

		[Test]
		public void persisting_an_order_should_log_message()
		{
			var purchaseOrder = new PurchaseOrder
			{
				Date = DateTime.Now,
				Number = "N1"
			};

			Assert.That(Spying.Logger(typeof(MyConversationCreationInterceptor<>))
					.Execute(() => modifyOrderModel.Persist(purchaseOrder))
					.MessageSequence,
					Is.EqualTo(new[] { "My conversation ended" }));
		}

		[Test]
		public void find_or_create_new_will_create_a_new_order_for_nonexisting_one()
		{
			var order = modifyOrderModel.FindOrderOrCreateNew("A0001", DateTime.Now);
			Assert.That(order, Is.Not.Null);
			Assert.That(order.IsNew, Is.True);
			modifyOrderModel.EndConversation();
		}

		[Test]
		public void find_or_create_new_will_obtain_an_existing_order()
		{
			var orderDateTime = DateTime.Now;
			var orderNumber = "A0001";

			var order = modifyOrderModel.FindOrderOrCreateNew(orderNumber, orderDateTime);
			modifyOrderModel.Persist(order);

			var order2 = modifyOrderModel.FindOrderOrCreateNew(orderNumber, orderDateTime);
			Assert.That(order2, Is.Not.Null);
			Assert.That(order2.IsNew, Is.False);
			modifyOrderModel.EndConversation();
		}

		#region Overrides of TestCase

		protected override IList Mappings
		{
			get { return new ArrayList(); }
		}

		#endregion
	}
}
