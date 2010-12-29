using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel;
using Microsoft.Practices.ServiceLocation;

namespace uNhAddIns.PostSharpAdapters.Tests.Util
{
	public class ServiceLocatorStub : ServiceLocatorImplBase
	{
		private readonly IKernel kernel = new DefaultKernel();

		private ServiceLocatorStub()
		{ }

		public ServiceLocatorStub AddInstance<TService>(TService instance)
		{
			kernel.AddComponentInstance(typeof (TService).Name, typeof (TService), instance);
			return this;
		}

		public ServiceLocatorStub AddService<TService, TImplementor>()
		{
			kernel.AddComponent(typeof(TService).Name, typeof(TService), typeof(TImplementor));
			return this;
		}

		public ServiceLocatorStub AddService<TImplementor>()
		{
			kernel.AddComponent(typeof(TImplementor).Name, typeof(TImplementor));
			return this;
		}

		public static ServiceLocatorStub Create()
		{
			var fakeServiceLocator = new ServiceLocatorStub();
			ServiceLocator.SetLocatorProvider(() => fakeServiceLocator);
			return fakeServiceLocator;
		}

		protected override object DoGetInstance(Type serviceType, string key)
		{
			return string.IsNullOrEmpty(key) ? kernel.Resolve(serviceType) : kernel.Resolve(key, serviceType);
		}

		protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
		{
			return kernel.ResolveAll(serviceType).Cast<object>();
		}
	}
}