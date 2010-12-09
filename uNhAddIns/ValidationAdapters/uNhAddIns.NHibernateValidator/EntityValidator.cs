using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate.Validator.Engine;
using uNhAddIns.Adapters;

namespace uNhAddIns.NHibernateValidator
{
	public class EntityValidator : IEntityValidator
	{
		private readonly ValidatorEngine validatorEngine;

		public EntityValidator(ValidatorEngine validatorEngine)
		{
			this.validatorEngine = validatorEngine;
		}

		#region IEntityValidator Members

		public bool IsValid(object entityInstance)
		{
			return validatorEngine.IsValid(entityInstance);
		}

		public IList<IInvalidValueInfo> Validate(object entityInstance)
		{
			return
				validatorEngine.Validate(entityInstance)
				.Select(iv => new InvalidValueInfo(iv))
				.Cast<IInvalidValueInfo>().ToList();
		}

		public IList<IInvalidValueInfo> Validate<T, TP>(T entityInstance, Expression<Func<T, TP>> property) where T : class
		{
			return
				validatorEngine.ValidatePropertyValue(entityInstance, property)
				.Select(iv => new InvalidValueInfo(iv))
				.Cast<IInvalidValueInfo>().ToList();
		}

		public IList<IInvalidValueInfo> Validate(object entityInstance, string property)
		{
			return
				validatorEngine.ValidatePropertyValue(entityInstance, property)
				.Select(iv => new InvalidValueInfo(iv))
				.Cast<IInvalidValueInfo>().ToList();
		}

		#endregion
	}
}