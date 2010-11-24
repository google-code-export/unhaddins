using log4net;
using uNhAddIns.SessionEasier.Conversations;

namespace SessionManagement.Domain.Impl
{
	public class MyConversationCreationInterceptor<T> : IConversationCreationInterceptorConvention<T> where T : class
	{
		private static readonly ILog log = LogManager.GetLogger(typeof (MyConversationCreationInterceptor<>));
		#region Implementation of IConversationCreationInterceptor

		public void Configure(IConversation conversation)
		{
			conversation.Ended += conversation_Ended;
		}

		void conversation_Ended(object sender, EndedEventArgs e)
		{
			if (!e.Disposing)
			{
				log.Debug("My conversation ended");
			}
		}

		#endregion
	}
}