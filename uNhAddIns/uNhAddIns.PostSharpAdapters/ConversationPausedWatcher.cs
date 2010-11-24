using System;
using System.Collections.Generic;
using uNhAddIns.SessionEasier.Conversations;

namespace uNhAddIns.PostSharpAdapters
{
	public class ConversationPausedWatcher
	{
		private readonly IDictionary<IConversation, bool> pausedConversations =
			new Dictionary<IConversation, bool>();

		public void Watch(IConversation conversation)
		{
			conversation.Paused += ConversationPaused;
			conversation.Resumed += ConversationResumed;
			conversation.Ended += ConversationEnded;
			pausedConversations[conversation] = false;
		}

		public bool IsPaused(IConversation conversation)
		{
			return pausedConversations.ContainsKey(conversation) && pausedConversations[conversation];
		}

		void ConversationEnded(object sender, EndedEventArgs e)
		{
			pausedConversations.Remove((IConversation) sender);
		}

		private void ConversationResumed(object sender, EventArgs e)
		{
			pausedConversations[(IConversation)sender] = false;
		}

		private void ConversationPaused(object sender, EventArgs e)
		{
			pausedConversations[(IConversation) sender] = true;
		}
	}
}