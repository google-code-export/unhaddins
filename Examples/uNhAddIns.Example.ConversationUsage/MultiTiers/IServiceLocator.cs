namespace uNhAddIns.Example.ConversationUsage.MultiTiers
{
	public interface IServiceLocator
	{
		T Resolve<T>(string key);
		T Resolve<T>();
	}
}