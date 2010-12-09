using System.Linq;
using NUnit.Framework;
using uNhAddIns.DataAnnotations.Engine;

namespace uNhAddIns.DataAnnotations.Tests
{
	[TestFixture]
	public class EntityMetaDataFixture
	{
		[Test]
		public void can_get_validations()
		{
			var metadata = new EntityMetadata(typeof (Person));

			var rangeValidation = metadata.ValidationsPerPropertyName["Age"]
										.First();

			rangeValidation.ErrorMessage.Should().Be.EqualTo(Person.AgeErrorMessage);
		}
	}
}