using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using uNhAddIns.Adapters;
using System.ComponentModel.DataAnnotations;
using uNhAddIns.DataAnnotations.Engine;

namespace uNhAddIns.DataAnnotations
{
	public class EntityValidator : IEntityValidator
	{
		private static readonly DataAnnotationsEntityInspector inspector =
							new DataAnnotationsEntityInspector();

		#region IEntityValidator Members

		///<summary>
		/// Returns true if the entity is valid.
		///</summary>
		///<param name="entityInstance"></param>
		///<returns></returns>
		public bool IsValid(object entityInstance)
		{
			var metadata = inspector.GetMetadata(entityInstance.GetType());
			var isValid = metadata.ValidationsPerProperty
				.All(propertyValidator => 
					!propertyValidator.Value.Any(v => 
						!v.IsValid(propertyValidator.Key.GetValue(entityInstance, null))));

			return isValid;
		}

		///<summary>
		/// Validates an entity and returns the information about invalid values.
		/// </summary>
		///<param name="entityInstance"></param>
		///<returns></returns>
		public IList<IInvalidValueInfo> Validate(object entityInstance)
		{
			var result = inspector.GetMetadata(entityInstance.GetType())
				.ValidationsPerProperty
				.Select(kv => new
				              	{
				              		Property = kv.Key,
				              		Validators = kv.Value,
				              		Value = kv.Key.GetValue(entityInstance, null)
				              	})
				.SelectMany(pvv =>
				            pvv.Validators
				            	.Where(v => !v.IsValid(pvv.Value))
				            	.Select(v => new InvalidValueInfo(entityInstance.GetType(), pvv.Property.Name, v))
				).Cast<IInvalidValueInfo>().ToList();

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
			var propertyInfo = DecodeMemberAccessExpression(property) as PropertyInfo;
			if(propertyInfo == null)
			{
				throw new InvalidOperationException("The expression should be a property");
			}

			var value = propertyInfo.GetValue(entityInstance, null);
			var entityType = typeof(T);
			var validators = inspector.GetMetadata(entityType)
				.ValidationsPerProperty[propertyInfo];
			var result = Validate(validators, entityType, value, propertyInfo);

			return result;
		}

		private static List<IInvalidValueInfo> Validate(
			IEnumerable<ValidationAttribute> validators, 
			Type entityType, object value, 
			PropertyInfo propertyInfo)
		{
			return validators
				.Where(v => !v.IsValid(value))
				.Select(v => new InvalidValueInfo(entityType, propertyInfo.Name, v))
				.Cast<IInvalidValueInfo>().ToList();
		}


		public static MemberInfo DecodeMemberAccessExpression<TEntity, TResult>(Expression<Func<TEntity, TResult>> expression)
		{
			if (expression.Body.NodeType != ExpressionType.MemberAccess)
			{
				if ((expression.Body.NodeType == ExpressionType.Convert) && (expression.Body.Type == typeof(object)))
				{
					return ((MemberExpression)((UnaryExpression)expression.Body).Operand).Member;
				}
				throw new InvalidOperationException(
					string.Format("Invalid expression type: Expected ExpressionType.MemberAccess, Found {0}", expression.Body.NodeType));
			}
			return ((MemberExpression)expression.Body).Member;
		}

		///<summary>
		/// Validates a property of the entity and returns the information about invalid values.
		///</summary>
		///<param name="entityInstance"></param>
		///<param name="property"></param>
		///<returns></returns>
		public IList<IInvalidValueInfo> Validate(object entityInstance, string property)
		{
			var entityType = entityInstance.GetType();
			var propertyInfo = entityType.GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
											
			if(propertyInfo == null)
			{
				throw new InvalidOperationException("The expression should be a property");
			}

			var value = propertyInfo.GetValue(entityInstance, null);
			
			var validators = inspector.GetMetadata(entityType)
				.ValidationsPerProperty[propertyInfo];
			var result = Validate(validators, entityType, value, propertyInfo);

			return result;
		}

		#endregion
	}
}