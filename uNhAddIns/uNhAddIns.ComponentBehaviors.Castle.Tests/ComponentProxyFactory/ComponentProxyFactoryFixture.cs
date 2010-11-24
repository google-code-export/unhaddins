using System;
using System.Collections.Generic;
using System.ComponentModel;
using Castle.MicroKernel;
using Moq;
using NUnit.Framework;
using uNhAddIns.ComponentBehaviors.Castle.Configuration;

namespace uNhAddIns.ComponentBehaviors.Castle.Tests.ComponentProxyFactory
{
	[TestFixture]
	public class ComponentProxyFactoryFixture
	{
		[Test]
		public void get_entity_should_work()
		{
			//I will not mock the proxy factory and I will not mock any interceptor.

			var behaviorConfig = new Mock<IBehaviorConfigurator>();
			behaviorConfig.Setup(b => b.GetProxyInformation(typeof (A)))
				.Returns(new ProxyInformation(typeof (A),
				                              new List<Type> {typeof (INotifyPropertyChanged)},
				                              new List<Type> {typeof (NotifyPropertyChangedBehavior)}))
				.AtMostOnce();

			var kernelMock = new Mock<IKernel>();
			kernelMock.Setup(k => k[typeof(NotifyPropertyChangedBehavior)])
				.Returns(new NotifyPropertyChangedBehavior());

			var componentProxyFactory = new Castle.ComponentProxyFactory.ComponentProxyFactory(kernelMock.Object,
			                                                                                   behaviorConfig.Object);

			var proxy = componentProxyFactory.GetEntity<A>();

			proxy.Should().Be.AssignableTo<A>();
			proxy.Should().Be.AssignableTo<INotifyPropertyChanged>();
		}

		[Test]
		public void when_entity_hasnot_behaviors_then_doesnt_generate_a_proxy()
		{
			//I will not mock the proxy factory and I will not mock any interceptor.

			var behaviorConfig = new Mock<IBehaviorConfigurator>();
			behaviorConfig.Setup(b => b.GetProxyInformation(typeof(A)))
				.Returns(new ProxyInformation(typeof(A),
											  new List<Type>(),
											  new List<Type>()))
				.AtMostOnce();

			var kernelMock = new Mock<IKernel>();
			
			var componentProxyFactory = new Castle.ComponentProxyFactory.ComponentProxyFactory(kernelMock.Object,
																							   behaviorConfig.Object);

			var proxy = componentProxyFactory.GetEntity<A>();
			proxy.GetType().Should().Be.EqualTo(typeof (A));
		}

	}

	public class A
	{
	}
}