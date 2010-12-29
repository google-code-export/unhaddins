using System;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using uNhAddIns.Adapters;

namespace uNhAddIns.VAB
{
	public class InvalidValueInfo : IInvalidValueInfo
	{
		public InvalidValueInfo(ValidationResult validationResult)
		{
			Message = validationResult.Message;
			PropertyName = validationResult.Key;
			EntityType = validationResult.Target.GetType();
		}

		/// <summary>
		/// This is the class type that the validation result is applicable to. For instance,
		/// if the validation result concerns a duplicate record found for an employee, then
		/// this property would hold the typeof(Employee). It should be expected that this
		/// property will never be null.
		/// </summary>
		public Type EntityType { get; private set; }

		/// <summary>
		/// If the validation result is applicable to a specific property, then this
		/// <see cref="PropertyInfo" /> would be set to a property name.
		/// </summary>
		public string PropertyName { get; private set; }

		/// <summary>
		/// Holds the message describing the validation result for the EntityType and/or PropertyContext
		/// </summary>
		public string Message { get; private set; }
	}
}