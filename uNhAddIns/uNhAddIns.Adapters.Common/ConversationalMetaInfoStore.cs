using System;
using System.Collections.Generic;

namespace uNhAddIns.Adapters.Common
{
	public class ConversationalMetaInfoStore : IConversationalMetaInfoStore
	{
		private readonly IDictionary<Type, IConversationalMetaInfoHolder> typeInfo =
			new Dictionary<Type, IConversationalMetaInfoHolder>(100);

		private readonly object locker = new object();

		#region IConversationalMetaInfoStore Members

		public IConversationalMetaInfoHolder GetMetadataFor(Type conversationalClass)
		{
			IConversationalMetaInfoHolder result;
			typeInfo.TryGetValue(conversationalClass, out result);
			return result;
		}

		public IEnumerable<IConversationalMetaInfoHolder> MetaData
		{
			get { return typeInfo.Values; }
		}

		public void AddMetadata(IConversationalMetaInfoHolder classMetadata)
		{
			if (classMetadata == null)
			{
				throw new ArgumentNullException("classMetadata");
			}
			lock (locker)
			{
				typeInfo.Add(classMetadata.ConversationalClass, classMetadata);				
			}
		}

		#endregion
	}
}