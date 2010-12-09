namespace uNhAddIns.Example.ConversationUsage.MultiTiers
{
	public class ServiceLocator
	{
		private static IServiceLocator soleInstance;

		public static void Load(IServiceLocator arg)
		{
			soleInstance = arg;
		}

		public static T GetService<T>(string key)
		{
			return soleInstance.Resolve<T>(key);
		}

		public static T GetService<T>()
		{
			return soleInstance.Resolve<T>();
		}
	}
}