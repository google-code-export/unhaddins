using Castle.Windsor;
using ChinookMediaManager.GuyWire.Configurators;
using NUnit.Framework;
using SharpTestsEx;
using uNhAddIns.Adapters;

namespace ChinookMediaManager.Domain.Test.ValidationTests
{
	[TestFixture]
	public class AlbumValidationFixture
	{
		private IWindsorContainer container;
		private IEntityValidator entityValidator;

		[TestFixtureSetUp]
		public void FixtureSetUp()
		{
			container = new WindsorContainer();
			container.Install(new ValidatorInstaller());
			entityValidator = container.GetService<IEntityValidator>();
		}

		[Test]
		public void WhenTitleIsNullThenValidationResultIsNotEmpty()
		{
			var album = new Album {Title = null};
			entityValidator.Validate(album, a => a.Title).Should().Not.Be.Empty();
		}

		[Test]
		public void WhenTitleIsToLargeThenValidationResultIsNotEmpty()
		{
			var album = new Album { Title = new string('j',201)};
			entityValidator.Validate(album, a => a.Title).Should().Not.Be.Empty();
		}

	}
}