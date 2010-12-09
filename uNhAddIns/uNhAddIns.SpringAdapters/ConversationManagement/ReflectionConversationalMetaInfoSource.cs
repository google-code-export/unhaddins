using System.Reflection;
using uNhAddIns.Adapters.Common;

namespace uNhAddIns.SpringAdapters.ConversationManagement
{
	public class ReflectionConversationalMetaInfoSource : ReflectionConversationalMetaInfoStore
	{
		private const BindingFlags MethodBindingFlags =
			BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.IgnoreCase
			| BindingFlags.FlattenHierarchy;

		protected override void BuildMetaInfoFromType(ConversationalMetaInfoHolder metaInfo, System.Type implementation)
		{
			MethodInfo[] methods = implementation.GetMethods(MethodBindingFlags);

			foreach (MethodInfo method in methods)
			{
				var mi = MetaInfoInspector.GetMethodInfo(method);
				AddMethodIfNecessary(metaInfo, method, mi);
			}
		}
	}
}