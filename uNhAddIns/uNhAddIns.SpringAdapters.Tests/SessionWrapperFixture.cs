using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using Spring.Context;
using Spring.Context.Support;
using uNhAddIns.Adapters.CommonTests.Integration;
using uNhAddIns.SessionEasier;

namespace uNhAddIns.SpringAdapters.Tests
{
	[TestFixture]
	public class SessionWrapperFixture : SessionWrapperFixtureBase
	{

		#region Overrides of SessionWrapperFixtureBase

		protected override IServiceLocator NewServiceLocator()
		{
			IConfigurableApplicationContext context = new StaticApplicationContext();
			var objectFactory = context.ObjectFactory;
			objectFactory.RegisterSingleton(typeof(ISessionWrapper).FullName, new SessionWrapper());

			return new SpringServiceLocatorAdapter(objectFactory);
		}

		#endregion
	}
}