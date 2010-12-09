namespace uNhAddIns.CastleValidator
{
	using System;
	using Adapters;

	public class InvalidValueInfo : IInvalidValueInfo
	{
		public InvalidValueInfo(Type entityType, string propertyName, string message)
		{
			EntityType = entityType;
			PropertyName = propertyName;
			Message = message;
		}

		public Type EntityType { get; private set; }

		public string PropertyName { get; private set; }

		public string Message { get; private set; }
	}
}
