using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Castle.Facilities.FactorySupport;
using Castle.Facilities.WcfIntegration;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NHibernate;
using SessionManagement.Data.NH.Repositories;
using SessionManagement.Data.NH.SchemaCreation;
using SessionManagement.Data.Repositories;
using SessionManagement.Domain.Impl;
using SessionManagement.Domain.Model;
using SessionManagement.Infrastructure.InversionOfControl;
using uNhAddIns.CastleAdapters;
using uNhAddIns.SessionEasier;
using uNhAddIns.SessionEasier.Contexts;

namespace SessionManagement.WCF.Host
{
	class Program
	{
		private static ComponentRegistration<IEndpointBehavior> sessionEndPoint;

		private static ComponentRegistration<IEndpointBehavior> SessionEndPoint
		{
			get
			{
				if (sessionEndPoint == null)
					sessionEndPoint = Component.For<IEndpointBehavior>()
						.ImplementedBy<SessionEndPointBehavior>();

				return sessionEndPoint;
			}
		}

		static void Main(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
			var schemaCreator = new SchemaCreator();
			schemaCreator.CreateSchema();

			var container = new WindsorContainer()
				.AddFacility<WcfFacility>()
				.AddFacility<FactorySupportFacility>()
				.Register(SessionEndPoint)
				.Register(Component.For<IProductRepository>().ImplementedBy<ProductRepository>())
				.Register(Service<IProductModel, ProductModel>(9595));

			var nhConfigurator = new DefaultSessionFactoryConfigurationProvider();
			var sfp = new SessionFactoryProvider(nhConfigurator);
			container.Register(Component.For<ISessionFactoryProvider>().Instance(sfp));
			container.Register(Component.For<ISessionWrapper>().ImplementedBy<SessionWrapper>());
			container.Register(Component.For<ISessionFactory>().Instance(sfp.GetFactory(null)));

			IoC.RegisterResolver(new WindsorDependencyResolver(container));

			CurrentSessionContext.Wrapper = container.Resolve<ISessionWrapper>();

			Console.WriteLine("Server started");
			Console.ReadLine();
			schemaCreator.CreateSchema();
		}

		static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Console.WriteLine(e.ExceptionObject.ToString());
		}

		private static ComponentRegistration<T> Service<T, U>(int port) where U : T
		{
			var component = Component.For<T>().ImplementedBy<U>();

			var name = typeof(T).Name;
			var at = string.Format("net.tcp://{0}:{1}/{2}", "localhost", port, name.Substring(1, name.Length - 1));
			Console.WriteLine(at);
			return
				component.ActAs(
					new DefaultServiceModel()
						.AddEndpoints(WcfEndpoint.BoundTo(new NetTcpBinding(SecurityMode.None){PortSharingEnabled = true} ).At(at))
					);
		}
	}
}