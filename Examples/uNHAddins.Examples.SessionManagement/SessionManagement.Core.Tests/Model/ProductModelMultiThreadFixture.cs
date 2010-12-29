using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NHibernate;
using NUnit.Framework;
using SessionManagement.Domain.Model;
using SessionManagement.Infrastructure.InversionOfControl;
using SessionManagement.TestUtils;

// This class is here to test some multithreading issues that were solved in uNHAddins
namespace SessionManagement.Domain.Tests.Model
{
	internal class Worker
	{
		#region methods

		public void DoWork()
		{
			var productModel = IoC.Resolve<IProductModel>();
			Assert.IsNotNull(productModel, "Couldn't obtain the model");

			int id = count++;

			Product savedProduct =
				productModel.Save(new Product
				{
					Code = string.Format("A{0}", id),
					Description = string.Format("A{0} Product", id),
					Price = 17.25
				});

			Assert.That(savedProduct, Is.Not.Null);
			Assert.That(savedProduct.Id > 0);
		}

		#endregion

		#region field

		internal static int count;

		#endregion
	}

	[TestFixture]
	public class ProductModelMultiThreadFixture : TestCase
	{
		private readonly object locker = new object();

		private void DoWork()
		{
			lock (locker)
			{
				var worker = new Worker();
				worker.DoWork();
			}
		}

		protected override IList Mappings
		{
			get { return new ArrayList(); }
		}

		private static void AssertDuplicates(IEnumerable<Product> list)
		{
			IList<string> codes = new List<string>();
			foreach (var product in list)
			{
				Assert.That(!codes.Contains(product.Code), "Found duplicate codes");
				codes.Add(product.Code);
			}
		}

		[Test]
		public void ExistenRepetidosNOTest()
		{
			IList<Product> products = new List<Product>
			                 	{
			                 		new Product {Code = "A1"},
			                 		new Product {Code = "A2"},
			                 		new Product {Code = "A3"},
			                 	};

			AssertDuplicates(products);
		}

		[Test]
		public void ExistenRepetidosSITest()
		{
			IList<Product> products = new List<Product>
			                 	{
			                 		new Product {Code = "A1"},
			                 		new Product {Code = "A2"},
			                 		new Product {Code = "A2"},
			                 	};

			Assert.Throws<AssertionException>(() => AssertDuplicates(products));
		}

		[Test]
		public void OneWork()
		{
			DoWork();

			using (var session = sessions.OpenSession())
			{
				Assert.AreEqual(1, session.CreateQuery("from Product").List().Count);
				session.Delete("from Product");
				session.Flush();
			}
		}

		[Test]
		public void ParallelWorks()
		{
			const int max = 5;

			for (int i = 0; i < max; i++)
			{
				var thread = new Thread(DoWork);
				thread.Start();
				Thread.Sleep(20);
			}

			Thread.CurrentThread.Join(1000);

			using (ISession session = sessions.OpenSession())
			{
				var list = session.CreateQuery("from Product").List<Product>();

				Assert.AreEqual(max, list.Count, "Number of duplicated elements does not match the number of threads");
				AssertDuplicates(list);

				session.Delete("from Product");
				session.Flush();
			}
		}
	}
}