using System;

namespace SessionManagement.Domain.Events
{
	public class EventSource
	{
		public static event EventHandler ConversationEnded;

		private static void InvokeConversationEnded(EventArgs e)
		{
			var conversationEndedHandler = ConversationEnded;
			if (conversationEndedHandler != null)
			{
				conversationEndedHandler(new object(), e);
			}
		}

		public static void Conversation_Ended(object sender, EventArgs e)
		{
			InvokeConversationEnded(EventArgs.Empty);
		}
	}
}