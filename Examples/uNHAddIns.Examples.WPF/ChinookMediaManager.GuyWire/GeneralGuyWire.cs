using System;
using System.Linq;
using Castle.Facilities.FactorySupport;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.Practices.ServiceLocation;
using uNhAddIns.Adapters;
using uNhAddIns.CastleAdapters.AutomaticConversationManagement;

namespace ChinookMediaManager.GuyWire
{
	public class GuyWire : IGuyWire
	{
		private IWindsorContainer container;

		#region IGuyWire Members

		/// <summary>
		/// Application wire.
		/// </summary>
		/// <remarks>
		/// IoC container configuration (more probably conf. by code).
		/// </remarks>
		public void Wire()
		{
			if (container != null)
				Dewire();
			container = new WindsorContainer();
			container.AddFacility<FactorySupportFacility>();
			container.AddFacility<PersistenceConversationFacility>();

			var windsorInstallers = typeof (GuyWire).Assembly.GetTypes()
				.Where(t => !t.IsAbstract && !t.IsInterface && typeof (IWindsorInstaller).IsAssignableFrom(t))
				.Select(t => Activator.CreateInstance(t))
				.OfType<IWindsorInstaller>().ToArray();

			container.Install(windsorInstallers);

			//I don't like this very much, but it is the only way to go with caliburn for now.
			ServiceLocator.SetLocatorProvider(() => new Caliburn.Windsor.WindsorAdapter(container));
			log4net.Config.XmlConfigurator.Configure();
		}

		/// <summary>
		/// Application dewire
		/// </summary>
		/// <remarks>
		/// IoC container dispose.
		/// </remarks>
		public void Dewire()
		{
			if (container != null)
				container.Dispose();
		}

		#endregion
	}
}