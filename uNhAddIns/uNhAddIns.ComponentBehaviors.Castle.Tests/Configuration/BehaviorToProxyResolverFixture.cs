using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Moq;
using NUnit.Framework;
using uNhAddIns.ComponentBehaviors.Castle.Configuration;
using uNhAddIns.NHibernateTypeResolver;

namespace uNhAddIns.ComponentBehaviors.Castle.Tests.Configuration
{
	public class A
	{
	}

	public abstract class B : A, INotifyPropertyChanged, IDataErrorInfo, IEditableObject
	{
		#region IDataErrorInfo Members

		public abstract string this[string columnName] { get; }


		public abstract string Error { get; }

		#endregion

		#region IEditableObject Members

		public abstract void BeginEdit();


		public abstract void EndEdit();


		public abstract void CancelEdit();

		#endregion

		#region INotifyPropertyChanged Members

		public abstract event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}

	[TestFixture]
	public class BehaviorToProxyResolverFixture
	{
		[Test]
		public void behavior_configurator_should_work()
		{
			var behaviorStore = new Mock<IBehaviorStore>();

			var behaviorsTypes = new HashSet<Type>
			               	{
								typeof (NotifyPropertyChangedBehavior),
			               		typeof (DataErrorInfoBehavior),
			               		typeof (EditableBehavior)
			               	};

			behaviorStore.Setup(bs => bs.GetBehaviorsForType(typeof (A)))
							.Returns(behaviorsTypes);

			var additionalInterfaces = new[]
			                	{
			                		typeof(IDataErrorInfo),
			                		typeof(INotifyPropertyChanged),
			                		typeof(IEditableObject),
			                		typeof(IWellKnownProxy)
			                	};

			var behaviorToProxyResolver = new BehaviorConfigurator(behaviorStore.Object);

			var proxyInfo = behaviorToProxyResolver.GetProxyInformation(typeof (A));

			behaviorsTypes.All(b => proxyInfo.Interceptors.Contains(b)).Should().Be.True();

			additionalInterfaces.All(i => proxyInfo.AdditionalInterfaces.Contains(i)).Should().Be.True();
		}

		[Test]
		public void already_implemented_interfaces_in_target_are_evicted()
		{
			var behaviorStore = new Mock<IBehaviorStore>();

			behaviorStore.Setup(bs => bs.GetBehaviorsForType(typeof (B)))
				.Returns(new HashSet<Type>
				         	{
				         		typeof (NotifyPropertyChangedBehavior),
				         		typeof (DataErrorInfoBehavior),
				         		typeof (EditableBehavior)
				         	});

			var behaviorToProxyResolver = new BehaviorConfigurator(behaviorStore.Object);
			//Method under test.
			var proxyInfo = behaviorToProxyResolver.GetProxyInformation(typeof (B));
			proxyInfo.AdditionalInterfaces.Should()
				.Not.Contain(typeof (IDataErrorInfo))
				.And.Not.Contain(typeof (IEditableObject))
				.And.Not.Contain(typeof (INotifyPropertyChanged));
		}

		[Test]
		public void behaviors_none_should_return_empty_interceptors_and_additionalinterfaces()
		{
			var behaviorStore = new Mock<IBehaviorStore>();
			behaviorStore.Setup(bs => bs.GetBehaviorsForType(typeof (A)))
				.Returns(new HashSet<Type>());

			var behaviorToProxyResolver = new BehaviorConfigurator(behaviorStore.Object);
			//Method under test.
			var proxyInfo = behaviorToProxyResolver.GetProxyInformation(typeof (A));
			proxyInfo.AdditionalInterfaces.Should().Be.Empty();
			proxyInfo.Interceptors.Should().Be.Empty();
		}
	}
}