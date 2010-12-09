using System.Reflection;
using uNhAddIns.Entities;

namespace ChinookMediaManager.Domain.Test.Util
{
	internal static class EntityExtensions
	{
		private const BindingFlags Flags =
			BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

		private const string IdFieldName = "<Id>k__BackingField";

		private static readonly FieldInfo IdField = typeof(AbstractEntity<int>).GetField(IdFieldName, Flags);

		public static T SetId<T>(this T entity, int value) where T : Entity
		{
			IdField.SetValue(entity, value);
			return entity;
		}
	}
}