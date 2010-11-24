using NUnit.Framework;

namespace uNhAddIns.PostSharpAdapters.Tests
{
	[TestFixture]
	public class WhenEntityHasDefaultEndModeAndMethodHasAttribute
	{
		[PersistenceConversation]
		public void SampleMethod()
		{}
	
		[Test]
		public void EndModeShouldBeEqualToEntityDefaultEndMode()
		{
			new PersistenceConversationalAttribute{DefaultEndMode = EndMode.End}
				.GetMethodEndMode(GetType().GetMethod("SampleMethod"))
				.Should().Be.EqualTo(EndMode.End);
		}
	}
}