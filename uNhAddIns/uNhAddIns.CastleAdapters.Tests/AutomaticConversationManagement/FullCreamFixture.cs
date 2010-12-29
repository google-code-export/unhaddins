using Castle.Facilities.FactorySupport;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using uNhAddIns.Adapters.CommonTests.Integration;

namespace uNhAddIns.CastleAdapters.Tests.AutomaticConversationManagement
{
	[TestFixture]
	public class FullCreamFixture : FullCreamFixtureBase
	{
		#region Overrides of FullCreamFixtureBase

		protected override void InitializeServiceLocator()
		{
			var container = new WindsorContainer(new XmlInterpreter());
			var sl = new WindsorServiceLocator(container);
			container.Register(Component.For<IServiceLocator>().Instance(sl));
			ServiceLocator.SetLocatorProvider(() => sl);
		}

		#endregion
	}
}