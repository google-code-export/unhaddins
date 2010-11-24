using SessionManagement.Domain.Events;
using uNhAddIns.SessionEasier;
using uNhAddIns.SessionEasier.Conversations;

namespace SessionManagement.Data.NH.Extensions
{
	public class NotifiableConversationFactory : IConversationFactory
	{
		private readonly ISessionFactoryProvider factoriesProvider;
		private readonly ISessionWrapper wrapper;

		public NotifiableConversationFactory(ISessionFactoryProvider factoriesProvider, ISessionWrapper wrapper)
		{
			this.factoriesProvider = factoriesProvider;
			this.wrapper = wrapper;
		}

		#region Implementation of IConversationFactory

		public IConversation CreateConversation()
		{
			return new NhConversation(factoriesProvider, wrapper);
		}

		public IConversation CreateConversation(string conversationId)
		{
			var result = new NhConversation(factoriesProvider, wrapper, conversationId);

			if (conversationId.StartsWith("Notify"))
			{
				result.Ended += EventSource.Conversation_Ended;
			}

			return result;
		}

		#endregion
	}
}
