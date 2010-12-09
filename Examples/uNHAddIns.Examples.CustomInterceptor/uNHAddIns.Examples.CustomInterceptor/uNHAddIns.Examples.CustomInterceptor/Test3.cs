using System;
using Castle.Core;
using Castle.Core.Interceptor;
using Castle.DynamicProxy;
using Castle.Facilities.FactorySupport;
using Castle.MicroKernel.Registration;
using NUnit.Framework;
using uNHAddIns.Examples.CustomInterceptor.Domain;
using uNHAddIns.Examples.CustomInterceptor.Infrastructure;
using uNHAddIns.Examples.CustomInterceptor.Infrastructure.InterceptorSelectors;
using uNHAddIns.Examples.CustomInterceptor.Infrastructure.MethodsInterceptors;

namespace uNHAddIns.Examples.CustomInterceptor
{
    [TestFixture]
    public class Test3 : BaseTest
    {
        protected override void ConfigureWindsorContainer()
        {
            container.AddFacility<FactorySupportFacility>();
            container.Register(Component.For<EditableObjectInterceptor>());
            container.Register(Component.For<PropertyChangeInterceptor>());

            container.Register(Component.For<IStore>()
                                   .Proxy.AdditionalInterfaces(typeof (IEditableCategoryModel))
                                   .NotifyOnPropertyChange()
                                   .EditableObjectBehavior()
                                   .EnableNhibernateEntityCompatibility()
                                   .ImplementedBy<Store>());

            container.Register(Component.For<IEditableCategoryModel>()
                                   .UsingFactoryMethod(() => WindsorRegistrationUtil.GenerateEditableModel<IEditableCategoryModel, IStore>(new Store())));
        }

        [Test]
        public void editable_behavior_should_work_when_canceledit()
        {
            var editableCategory = container.Resolve<IEditableCategoryModel>();
            editableCategory.Name = "Fruits";
            
            editableCategory.BeginEdit();
            editableCategory.Name = "Vegetables";
            editableCategory.CancelEdit();

            editableCategory.Name.Should().Be.EqualTo("Fruits");
        }
        
        [Test]
        public void editable_behavior_should_work_when_endedit()
        {
            var editableCategory = container.Resolve<IEditableCategoryModel>();
            editableCategory.Name = "Fruits";

            editableCategory.BeginEdit();
            editableCategory.Name = "Vegetables";
            editableCategory.EndEdit();

            editableCategory.Name.Should().Be.EqualTo("Vegetables");
        }
    }

}