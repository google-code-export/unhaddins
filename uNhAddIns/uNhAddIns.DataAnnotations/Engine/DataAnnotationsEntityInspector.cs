using System;
using System.Collections.Generic;

namespace uNhAddIns.DataAnnotations.Engine
{
	public class DataAnnotationsEntityInspector
	{
		private IDictionary<Type, EntityMetadata> cache 
			= new Dictionary<Type, EntityMetadata>();

		public EntityMetadata GetMetadata(Type type)
		{
			EntityMetadata result;
			if(!cache.TryGetValue(type, out result))
			{
				lock (cache)
				{
					cache[type] = result = new EntityMetadata(type);
				}
			}
			return result;
		}
	}
}