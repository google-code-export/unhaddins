using System;
using System.ServiceModel;
using Castle.Facilities.WcfIntegration;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using SessionManagement.Domain;
using SessionManagement.Domain.Model;
using SessionManagement.Infrastructure.InversionOfControl;
using System.Threading;

namespace SessionManagement.WCF.Client
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("When host is ready press a key");
			Console.ReadLine();

			var container = new WindsorContainer()
				.AddFacility<WcfFacility>()
				.Register(Client<IProductModel>("localhost", 9595));

			IoC.RegisterResolver(new WindsorDependencyResolver(container));

			var productModel = container.Resolve<IProductModel>();
			productModel.Save(new Product {Code = "1", Description = "2", Price = 0.5});
			productModel.Save(new Product { Code = "3", Description = "3", Price = 3.5 });
			productModel.Save(new Product { Code = "4", Description = "4", Price = 6.5 });

			var products = productModel.GetProducts();
			Console.WriteLine("Products in DB: " + products.Count);
			
			Console.ReadLine();
		}

		private static ComponentRegistration<T> Client<T>(string ip, int port)
		{
			var name = typeof(T).Name;
			var at = string.Format("net.tcp://{0}:{1}/{2}", ip, port, name.Substring(1, name.Length - 1));
			Console.WriteLine(at);
			return Component.For<T>()
				.ActAs(new DefaultClientModel
				{
					Endpoint = WcfEndpoint.BoundTo(new NetTcpBinding(SecurityMode.None) { PortSharingEnabled = true }).At(at)
				});
		}
	}
}
