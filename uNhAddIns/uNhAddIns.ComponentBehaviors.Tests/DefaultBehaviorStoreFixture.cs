using System;
using NUnit.Framework;

namespace uNhAddIns.ComponentBehaviors.Tests
{
	public class SampleBehavior 
	{
		public Type[] GetAdditionalInterfaces()
		{
			throw new NotImplementedException();
		}

		public int GetRelativeOrder()
		{
			throw new NotImplementedException();
		}
	}

	public class SecondSampleBehavior 
	{
		public Type[] GetAdditionalInterfaces()
		{
			throw new NotImplementedException();
		}
		public int GetRelativeOrder()
		{
			throw new NotImplementedException();
		}
	}

    [TestFixture]
    public class DefaultBehaviorStoreFixture
    {
        [Test]
        public void can_push_and_pull_multiples_behaviors()
        {
            var defaultBehaviorStore = new BehaviorDictionary();

        	defaultBehaviorStore.For<Decimal>()
				.Add<SampleBehavior>()
				.Add<SecondSampleBehavior>();
				
			var behaviors = defaultBehaviorStore.GetBehaviorsForType(typeof (Decimal));

        	behaviors.Should().Contain(typeof (SampleBehavior))
				.And.Contain(typeof (SecondSampleBehavior));
        }
    }
}