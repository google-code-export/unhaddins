using Microsoft.Practices.ServiceLocation;
using uNhAddIns.SessionEasier.Conversations;

namespace uNhAddIns.PostSharpAdapters.Tests.Model
{
	public interface ILogInterceptor { }

	public class LogInterceptor : IConversationCreationInterceptor, ILogInterceptor
	{
		private readonly MyLogger logger;

		public const string ConversationStarting = "conversation starting";
		public const string ConversationStarted = "conversation started";

		public LogInterceptor(MyLogger logger)
		{
			this.logger = logger;
		}

		public void Configure(IConversation conversation)
		{
			conversation.Starting += (s, e) => logger.AddMessage(ConversationStarting);
			conversation.Started += (s, e) => logger.AddMessage(ConversationStarted);
		}
	}

	public class FixedLogInterceptor : LogInterceptor
	{
		public FixedLogInterceptor()
			: base(ServiceLocator.Current.GetInstance<MyLogger>())
		{
		}
	}

	public class LogInterceptorConvention<TServiceToIntercept> :
		LogInterceptor, IConversationCreationInterceptorConvention<TServiceToIntercept>
		where TServiceToIntercept : class
	{
		public LogInterceptorConvention(MyLogger logger)
			: base(logger)
		{
		}
	}
}