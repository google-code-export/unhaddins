using System.Linq;
using System.Collections.Generic;
using uNhAddIns.Adapters;

namespace uNhAddIns.CastleValidator
{
	using System;
	using System.Linq.Expressions;
	using System.Reflection;
	using Castle.Components.Validator;

	public class EntityValidator : IEntityValidator
	{
		private readonly IValidatorRunner _validatorRunner;

		public EntityValidator(IValidatorRunner validatorRunner)
		{
			_validatorRunner = validatorRunner;
		}

		public bool IsValid(object entityInstance)
		{
			return _validatorRunner.IsValid(entityInstance);
		}

		public IList<IInvalidValueInfo> Validate(object entityInstance)
		{
			ErrorSummary errorSummary;

			if (_validatorRunner.IsValid(entityInstance) || (errorSummary = _validatorRunner.GetErrorSummary(entityInstance)) == null)
			{
				return Enumerable.Empty<IInvalidValueInfo>().ToList();
			}

			var ivs = errorSummary.InvalidProperties
				.SelectMany(p => from x in errorSummary.GetErrorsForProperty(p) select new InvalidValueInfo(entityInstance.GetType(), p, x))
				.OfType<IInvalidValueInfo>().ToList();

			return ivs;
		}

		public IList<IInvalidValueInfo> Validate<T, TP>(T entityInstance, Expression<Func<T, TP>> property) where T : class
		{
			var propertyName = DecodeMemberAccessExpression(property).Name;

			return Validate(entityInstance, propertyName);
		}

		public IList<IInvalidValueInfo> Validate(object entityInstance, string propertyName)
		{
			ErrorSummary errorSummary;

			if (_validatorRunner.IsValid(entityInstance) || (errorSummary = _validatorRunner.GetErrorSummary(entityInstance)) == null)
			{
				return Enumerable.Empty<IInvalidValueInfo>().ToList();
			}

			return errorSummary.GetErrorsForProperty(propertyName)
				.Select(p => new InvalidValueInfo(entityInstance.GetType(), propertyName, p))
				.OfType<IInvalidValueInfo>().ToList();
		}

		static MemberInfo DecodeMemberAccessExpression<TEntity, TResult>(Expression<Func<TEntity, TResult>> expression)
		{
			if (expression.Body.NodeType != ExpressionType.MemberAccess)
			{
				throw new ValidationException(
					string.Format("Invalid expression type: Expected ExpressionType.MemberAccess, Found {0}", expression.Body.NodeType));
			}
			return ((MemberExpression)expression.Body).Member;
		}
	}
}
