using Castle.Components.Validator;
using NUnit.Framework;

namespace uNhAddIns.CastleValidator.Tests
{
	[TestFixture]
	public class EntityValidatorFixture
	{
		[Test]
		public void is_valid_should_work()
		{
			var person = new Person();


			var entityValidator = new EntityValidator(new ValidatorRunner(new CachedValidationRegistry()));
			entityValidator.IsValid(person).Should().Be.False();

			person.Age = 20;
			person.Name = "José F. Romaniello";
			entityValidator.IsValid(person).Should().Be.True();
		}

		[Test]
		public void validate_entire_instance_should_work()
		{
			var person = new Person();

			var entityValidator = new EntityValidator(new ValidatorRunner(new CachedValidationRegistry()));

			var results = entityValidator.Validate(person);

			results.Count.Should().Be.EqualTo(2);

			results[0].PropertyName.Should().Be.EqualTo("Name");
			results[0].EntityType.Should().Be.EqualTo(typeof(Person));
			results[0].Message.Should().Be.EqualTo(Person.NameErrorMessage);

			results[1].PropertyName.Should().Be.EqualTo("Age");
			results[1].EntityType.Should().Be.EqualTo(typeof(Person));
			results[1].Message.Should().Be.EqualTo(Person.AgeErrorMessage);

			person.Name = "Jose";
			person.Age = 20;
			entityValidator.Validate(person).Should().Be.Empty();

		}

		[Test]
		public void validate_property_strongly_typed()
		{
			var person = new Person();
			var entityValidator = new EntityValidator(new ValidatorRunner(new CachedValidationRegistry()));
			var result = entityValidator.Validate(person, p => p.Name);
			result[0].PropertyName.Should().Be.EqualTo("Name");
			result[0].Message.Should().Be.EqualTo(Person.NameErrorMessage);
			result[0].EntityType.Should().Be.EqualTo(typeof (Person));

			person.Name = "jose";
			entityValidator.Validate(person, p => p.Name).Should().Be.Empty();
		}

		[Test]
		public void validate_property_not_strongly_typed()
		{
			var person = new Person();
			var entityValidator = new EntityValidator(new ValidatorRunner(new CachedValidationRegistry()));
			var result = entityValidator.Validate(person, "Name");

			result[0].PropertyName.Should().Be.EqualTo("Name");
			result[0].Message.Should().Be.EqualTo(Person.NameErrorMessage);
			result[0].EntityType.Should().Be.EqualTo(typeof(Person));

			person.Name = "jose";
			entityValidator.Validate(person, "Name").Should().Be.Empty();
		}
	}
}