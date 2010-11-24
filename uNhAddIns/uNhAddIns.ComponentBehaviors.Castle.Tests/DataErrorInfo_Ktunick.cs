using System;
using System.ComponentModel;
using Castle.Facilities.FactorySupport;
using Castle.Windsor;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;
using NUnit.Framework;
using uNhAddIns.Adapters;
using uNhAddIns.ComponentBehaviors.Castle.Configuration;
using uNhAddIns.NHibernateTypeResolver;
using uNhAddIns.NHibernateValidator;
using Component = Castle.MicroKernel.Registration.Component;

namespace uNhAddIns.ComponentBehaviors.Castle.Tests
{
	public class Customer : ICustomer
	{
		#region ICustomer Members

		public virtual string Mail { get; set; }

		#endregion
	}

	public interface ICustomer
	{
		string Mail { get; set; }
	}

	public class CustomerValidationDef : ValidationDef<ICustomer>
	{
		public CustomerValidationDef()
		{
			Define(p => p.Mail).IsEmail().WithMessage("Should be email address.");
			Define(p => p.Mail).MaxLength(2).WithMessage("Lenght should be 2.");
		}
	}

	[TestFixture]
	public class DataErrorInfoValidatorFixture_KTunick
	{
		private IWindsorContainer container;

		private static IEntityValidator CreateEntityValidator()
		{
			var configuration = new FluentConfiguration();
			configuration.Register(typeof (DataErrorInfoValidatorFixture).Assembly.ValidationDefinitions())
				.SetDefaultValidatorMode(ValidatorMode.UseExternal)
				.AddEntityTypeInspector<NHVTypeInspector>();


			var engine = new ValidatorEngine();
			engine.Configure(configuration);

			return new EntityValidator(engine);
		}

		[TestFixtureSetUp]
		public void FixtureSetUp()
		{
			container = new WindsorContainer();
			container.AddFacility<FactorySupportFacility>();

			//Add the facility
			container.AddFacility<ComponentBehaviorsFacility>();

			//configure behaviors
			var config = new BehaviorDictionary();

			config.For<Customer>().Add<DataErrorInfoBehavior>();

			container.Register(Component.For<IBehaviorStore>().Instance(config));

			//Register a mock instance of EntityValidator.
			container.Register(Component.For<IEntityValidator>()
			                   	.Instance(CreateEntityValidator())
			                   	.LifeStyle.Singleton);

			//Register the entity
			container.Register(Component.For<ICustomer>()
			                   	.ImplementedBy<Customer>()
			                   	.LifeStyle.Transient);
		}


		[Test]
		public void get_error_should_work()
		{
			var person = container.Resolve<ICustomer>();

			person.Mail = "aaaaaaa";

			string[] errorMessages = ((IDataErrorInfo) person).Error
				.Split(new[] {Environment.NewLine}, StringSplitOptions.None);

			errorMessages.Length.Should().Be.EqualTo(2);

			errorMessages.Should().Contain("Lenght should be 2.")
				.And.Contain("Should be email address.");
		}

		//[Test]
		//[Ignore("Until NHV team apply http://nhjira.koah.net/browse/NHV-56.")]
		//public void get_item_should_work()
		//{
		//    var person = container.Resolve<Person>();

		//    person.Mail = "aaaaaaa";

		//    string[] errorMessages = ((IDataErrorInfo)person)["Mail"]
		//        .Split(new[] { Environment.NewLine }, StringSplitOptions.None);

		//    errorMessages.Length.Should().Be.EqualTo(2);

		//    errorMessages.Should().Contain("Lenght should be 2.")
		//        .And.Contain("Should be email address.");
		//}
	}
}