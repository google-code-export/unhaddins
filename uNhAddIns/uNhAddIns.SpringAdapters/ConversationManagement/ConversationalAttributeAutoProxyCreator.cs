using System;
using Spring.Aop.Framework.AutoProxy;
using uNhAddIns.Adapters.Common;

namespace uNhAddIns.SpringAdapters.ConversationManagement
{
	public class ConversationalAttributeAutoProxyCreator : AbstractFilteringAutoProxyCreator
	{
		private readonly ReflectionConversationalMetaInfoStore store;

		public ConversationalAttributeAutoProxyCreator(IConversationalMetaInfoStore store)
		{
			this.store = (ReflectionConversationalMetaInfoStore)store;
		}

		protected override bool IsEligibleForProxying(Type objType, string name)
		{
			if (store.GetMetadataFor(objType) == null)
			{
				return store.Add(objType);
			}
			return true;
		}
	}
}