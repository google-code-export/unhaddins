using System;
using System.Collections.Generic;
using NUnit.Framework;
using uNhAddIns.DataAnnotations.Engine;

namespace uNhAddIns.DataAnnotations.Tests
{
	[TestFixture]
	public class DataAnnotationsEntityInspectorFixture
	{
		[Test]
		public void entity_metadata_is_stored_on_cache()
		{
			var inspector = new DataAnnotationsEntityInspector();
			var metadataInstance = inspector.GetMetadata(typeof (Person));
			
			inspector.FieldValue<IDictionary<Type, EntityMetadata>>("cache")
				.Values.Should().Contain(metadataInstance);

		}

		[Test]
		public void getmetadata_always_return_the_same_instance_of_metadata()
		{
			var inspector = new DataAnnotationsEntityInspector();
			inspector.GetMetadata(typeof (Person))
					.Should().Be.SameInstanceAs(inspector.GetMetadata(typeof (Person)));
		}
	}
}