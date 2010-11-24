using Castle.MicroKernel.Facilities;
using uNhAddIns.Adapters.Common;

namespace uNhAddIns.CastleAdapters.AutomaticConversationManagement
{
	public class PersistenceConversationFacility : AbstractFacility
	{
		protected override void Init()
		{
			Kernel.AddComponent("uNhAddIns.conversation.interceptor", typeof(ConversationInterceptor));
			Kernel.AddComponent("uNhAddIns.conversation.MetaInfoStore", typeof(IConversationalMetaInfoStore), typeof(ReflectionConversationalMetaInfoStore));
			Kernel.ComponentModelBuilder.AddContributor(new PersistenceConversationalComponentInspector());
		}
	}
}