using System;
using System.ComponentModel;
using System.Linq;
using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Proxy;
using Moq;
using NUnit.Framework;
using uNhAddIns.ComponentBehaviors.Castle.Configuration;

namespace uNhAddIns.ComponentBehaviors.Castle.Tests.Configuration
{
    public class SampleEntity
    {
    }

    [TestFixture]
    public class BehavioInspectorFixture
    {
		//[Test]
		//public void kernel_without_store_should_throw()
		//{
		//    var behaviorInspector = new BehaviorInspector();
		//    var kernel = new Mock<IKernel>();
		//    var componentModel = new ComponentModel("key", typeof (SampleEntity), typeof (SampleEntity));

		//    new Action(() => behaviorInspector.ProcessModel(kernel.Object, componentModel))
		//        .Should().Throw<InvalidOperationException>().And
		//        .ValueOf.Message.Should().Be.EqualTo("Missing IBehaviorConfigurator service.");
		//}

        [Test]
        public void process_model_should_work()
        {
            var kernel = new Mock<IKernel>();
            var behaviorConfigurator = new Mock<IBehaviorConfigurator>();

            
            behaviorConfigurator.Setup(b => b.GetProxyInformation(typeof (SampleEntity)))
                .Returns(new ProxyInformation
                             (typeof (SampleEntity),
                             new []{typeof (IDataErrorInfo), typeof (INotifyPropertyChanged)},
                             new []{typeof(NotifyPropertyChangedBehavior), typeof(DataErrorInfoBehavior)}
                             ));


			kernel.Setup(k => k.Resolve<IBehaviorConfigurator>())
				.Returns(behaviorConfigurator.Object);

            kernel.Setup(k => k.HasComponent(typeof(IBehaviorStore)))
                .Returns(true);

            var behaviorInspector = new BehaviorInspector();


            var componentModel = new ComponentModel("key", typeof (SampleEntity), typeof (SampleEntity));

            //method under test
            behaviorInspector.ProcessModel(kernel.Object, componentModel);
            //

					// does not compile with Castle 2.5.2
			//    var interceptorsTypes = componentModel.Interceptors.Select(ir => ir.ServiceType);
			//interceptorsTypes.Should().Contain(typeof (NotifyPropertyChangedBehavior))
			//            .And.Contain(typeof (DataErrorInfoBehavior));
										
            ProxyUtil.ObtainProxyOptions(componentModel, true).AdditionalInterfaces
                .Should().Contain(typeof(IDataErrorInfo))
                .And.Contain(typeof(INotifyPropertyChanged));
        }
    }
}