using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using uNhAddIns.Adapters;

namespace uNhAddIns.DataAnnotations
{
	public class InvalidValueInfo : IInvalidValueInfo
	{
		private readonly ValidationAttribute _validationAttribute;

		public InvalidValueInfo(Type entityType, string propertyName, ValidationAttribute validationAttribute)
		{
			_validationAttribute = validationAttribute;
			PropertyName = propertyName;
			EntityType = entityType;
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
		public string Message { 
			get
			{
				//Todo: resolve the message within resources if needed.
				return _validationAttribute.ErrorMessage;	
			} 
		}
	}
}