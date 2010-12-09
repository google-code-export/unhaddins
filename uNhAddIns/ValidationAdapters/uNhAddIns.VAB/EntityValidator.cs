using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using uNhAddIns.Adapters;

namespace uNhAddIns.VAB
{
	public class EntityValidator : IEntityValidator
	{
		#region IEntityValidator Members

		///<summary>
		/// Returns true if the entity is valid.
		///</summary>
		///<param name="entityInstance"></param>
		///<returns></returns>
		public bool IsValid(object entityInstance)
		{
			var results = DoValidation(entityInstance);
			return results.IsValid;
		}

		///<summary>
		/// Validates an entity and returns the information about invalid values.
		/// </summary>
		///<param name="entityInstance"></param>
		///<returns></returns>
		public IList<IInvalidValueInfo> Validate(object entityInstance)
		{
			ValidationResults vapResults = DoValidation(entityInstance);
			var result = ConvertErrors(vapResults);
			return result;
		}

		///<summary>
		/// Validates a property of the entity and returns the information about invalid values.
		///</summary>
		///<param name="entityInstance"></param>
		///<param name="property"></param>
		///<typeparam name="T"></typeparam>
		///<typeparam name="TP"></typeparam>
		///<returns></returns>
		public IList<IInvalidValueInfo> Validate<T, TP>(T entityInstance, Expression<Func<T, TP>> property) where T : class
		{
			string propertyName = GetMemberInfo(property).Name;
			return Validate(entityInstance, propertyName);
		}

		///<summary>
		/// Validates a property of the entity and returns the information about invalid values.
		///</summary>
		///<param name="entityInstance"></param>
		///<param name="property"></param>
		///<returns></returns>
		public IList<IInvalidValueInfo> Validate(object entityInstance, string property)
		{
			ValidationResults vapResults = DoValidation(entityInstance);
			var resultsForProperty = vapResults.Where(v => v.Key == property);
			return ConvertErrors(resultsForProperty);
		}

		#endregion

		protected virtual ValidationResults DoValidation(object value)
		{
			var factory = ValidationFactory.CreateValidator(value.GetType());
			return factory.Validate(value);
		}

		protected virtual IList<IInvalidValueInfo> ConvertErrors(IEnumerable<ValidationResult> validationResults)
		{
			return validationResults.Select(e => new InvalidValueInfo(e))
				.OfType<IInvalidValueInfo>()
				.ToList();
		}

		private static MemberInfo GetMemberInfo(LambdaExpression lambda)
		{
			return ((MemberExpression) lambda.Body).Member;
		}
	}
}