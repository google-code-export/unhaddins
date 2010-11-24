using System;
using NHibernate.Validator.Engine;
using uNhAddIns.Adapters;

namespace uNhAddIns.NHibernateValidator
{
	public class InvalidValueInfo : IInvalidValueInfo
	{
		private readonly InvalidValue invalidValue;

		public InvalidValueInfo(InvalidValue invalidValue)
		{
			this.invalidValue = invalidValue;
		}

		#region IInvalidValueInfo Members

		public Type EntityType
		{
			get { return invalidValue.EntityType; }
		}

		public string PropertyName
		{
			get { return invalidValue.PropertyName; }
		}

		public string Message
		{
			get { return invalidValue.Message; }
		}

		#endregion
	}
}