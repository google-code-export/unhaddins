using System;
using Microsoft.Practices.ServiceLocation;
using Moq;
using NUnit.Framework;

namespace uNhAddIns.WCF.Tests
{
	[TestFixture]
	public class ServiceLocatorInstanceProviderTest
	{
		public class MyService
		{
			
		}

		[Test]
		public void CtorProtection()
		{
			Assert.Throws<ArgumentNullException>(() => new ServiceLocatorInstanceProvider(null) );
		}

		[Test]
		public void CallServiceLocatorToGetServiceInstance()
		{
			var sl = new Mock<IServiceLocator>();
			ServiceLocator.SetLocatorProvider(() => sl.Object);

			var slip = new ServiceLocatorInstanceProvider(typeof (MyService));
			slip.GetInstance(null);

			sl.Verify(x => x.GetInstance(typeof (MyService)));
		}

		[Test]
		public void CallServiceLocatorToGetServiceInstance_whenCallWithMessage()
		{
			var sl = new Mock<IServiceLocator>();
			ServiceLocator.SetLocatorProvider(() => sl.Object);

			var slip = new ServiceLocatorInstanceProvider(typeof(MyService));
			slip.GetInstance(null, null);

			sl.Verify(x => x.GetInstance(typeof(MyService)));
		}

		[Test]
		public void WhenReleaseInstance_DisposeTheService()
		{
			var sl = new Mock<IServiceLocator>();
			var disposableService = new Mock<IDisposable>();
			ServiceLocator.SetLocatorProvider(() => sl.Object);

			var slip = new ServiceLocatorInstanceProvider(typeof(IDisposable));
			slip.ReleaseInstance(null, disposableService.Object);

			disposableService.Verify(x => x.Dispose());
		}
	}
}