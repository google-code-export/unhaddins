using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;

namespace uNhAddIns.ComponentBehaviors.Castle
{
	[Behavior(0, typeof (IEditableObject))]
	public class EditableBehavior : IInterceptor
	{
		readonly IDictionary<PropertyInfo, object> _tempValues
			= new Dictionary<PropertyInfo, object>();

		private object target;

		bool _isInEditMode;
		Dictionary<string, PropertyInfo> _properties;

		public virtual bool IsEditing
		{
			get { return _isInEditMode; }
		}

		#region IInterceptor Members

		public void Intercept(IInvocation invocation)
		{
			switch (invocation.Method.Name)
			{
				case "BeginEdit":
					BeginEdit();
					return;
				case "CancelEdit":
					CancelEdit();
					return;
				case "EndEdit":
					EndEdit(target);
					return;
				default:
					break;
			}

			if ((!invocation.Method.Name.StartsWith("get_") &&
			     !invocation.Method.Name.StartsWith("set_")) || !IsEditing)
			{
				invocation.Proceed();
				return;
			}

			if (_properties == null)
			{
				IEnumerable<PropertyInfo> propertyInfos = invocation.InvocationTarget.GetType().GetProperties(BindingFlags.Public |
				                                                                                              BindingFlags.Instance)
					.Where(p => p.CanWrite);
				//TODO: Enhance this.
				_properties = new Dictionary<string, PropertyInfo>();
				foreach (PropertyInfo propertyInfo in propertyInfos)
				{
					if (!_properties.ContainsKey(propertyInfo.Name))
						_properties[propertyInfo.Name] = propertyInfo;
				}
				target = invocation.InvocationTarget;
			}

			bool isSet = invocation.Method.Name.StartsWith("set_");
			string propertyName = invocation.Method.Name.Substring(4);
			PropertyInfo property;
			if (!_properties.TryGetValue(propertyName, out property))
			{
				invocation.Proceed();
				return;
			}

			if (isSet)
			{
				_tempValues[property] = invocation.Arguments[0];
			}
			else
			{
				invocation.Proceed();
				object value;
				if (_tempValues.TryGetValue(property, out value))
					invocation.ReturnValue = value;
			}
		}

		#endregion

		public void BeginEdit()
		{
			_isInEditMode = true;
		}

		public void CancelEdit()
		{
			_tempValues.Clear();
			_isInEditMode = false;
		}

		public void EndEdit(object target)
		{
			_isInEditMode = false;
			foreach (PropertyInfo property in _tempValues.Keys)
			{
				property.SetValue(target, _tempValues[property], null);
			}
		}
	}
}