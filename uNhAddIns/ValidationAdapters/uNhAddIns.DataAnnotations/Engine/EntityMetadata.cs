using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace uNhAddIns.DataAnnotations.Engine
{
	public class EntityMetadata
	{
		public EntityMetadata(Type type)
		{
			ValidationsPerProperty = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.ToDictionary(
					p => p, 
					p => (IEnumerable<ValidationAttribute>)p.GetCustomAttributes(true).OfType<ValidationAttribute>().ToList());
		}

		public IDictionary<PropertyInfo, IEnumerable<ValidationAttribute>> ValidationsPerProperty { get; private set; }

		public IDictionary<string, IEnumerable<ValidationAttribute>> ValidationsPerPropertyName
		{
			get
			{
				return ValidationsPerProperty.ToDictionary(kv => kv.Key.Name, kv => kv.Value);
			}
		}
	}
}