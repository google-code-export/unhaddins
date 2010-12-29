using NUnit.Framework;

namespace uNhAddIns.Entities.Tests
{
	[TestFixture]
	public class AbstractEntityFixture
	{
		[Test]
		public void NotEqualToAnyThingElse()
		{
			var e1 = EntityStub.NewWithId(1);

			Assert.That((object)null, Is.Not.EqualTo(e1));
			Assert.That(new object(), Is.Not.EqualTo(e1));
		}

		[Test]
		public void PersistentWithDifferentId()
		{
			const int id1 = 1;
			const int id2 = 2;
			var e1 = EntityStub.NewWithId(id1);
			var e2 = EntityStub.NewWithId(id2);

			e2.Should().Not.Be.EqualTo(e1);
			e2.Should().Be.EqualTo(e2);
			Assert.That(1, Is.Not.EqualTo(e1));
		}

		[Test]
		public void PersistentOfDifferentClassAndSameId()
		{
			EntityStubA.NewWithId(1).Should().Not.Be.EqualTo(EntityStub.NewWithId(1));
		}

		[Test]
		public void TransientsAreNotEquals()
		{
			(new EntityStub()).Should().Not.Be.EqualTo(new EntityStub());			
		}

		[Test]
		public void PersistentInheritedWithSameId()
		{
			EntityStubInherit.NewWithId(1).Should().Be.EqualTo(EntityStub.NewWithId(1));
		}

		[Test]
		[Description("The HashCode does not change changing the Id")]
		public void HashDontChange()
		{
			var e = new EntityStub();
			var oldHash = e.GetHashCode();
			e.SetId(100);
			e.GetHashCode().Should().Be.EqualTo(oldHash);
		}

		[Test]
		public void HashCode()
		{
			const int id1 = 1;
			(new EntityStub()).GetHashCode()
				.Should("transients should not have same hash")
				.Not.Be.EqualTo((new EntityStub()).GetHashCode());

			EntityStub.NewWithId(id1).GetHashCode()
				.Should("persistent should have same hash.")
				.Be.EqualTo(EntityStub.NewWithId(id1).GetHashCode());

			EntityStub.NewWithId(id1).GetHashCode()
				.Should("persistent inherited should have same hash.")
				.Be.EqualTo(EntityStubInherit.NewWithId(id1).GetHashCode());
		}
	}

	public class EntityStub : AbstractEntity<int>
	{
		public override int Id{get; protected set;}
		
		public static EntityStub NewWithId(int id)
		{
			return new EntityStub {Id = id};
		}

		public void SetId(int id)
		{
			Id = id;
		}
	}

	public class EntityStubA : AbstractEntity<int>
	{
		public override int Id { get; protected set; }

		public static EntityStubA NewWithId(int id)
		{
			return new EntityStubA { Id = id };
		}
	}

	public class EntityStubInherit : EntityStub
	{
		public new static EntityStubInherit NewWithId(int id)
		{
			return new EntityStubInherit { Id = id };
		}
	}
}