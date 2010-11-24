using System;
using System.Collections.Generic;
using Castle.Core.Interceptor;

namespace uNHAddIns.Examples.CustomInterceptor.Infrastructure.MethodsInterceptors
{

    public class EditableObjectInterceptor : IInterceptor
    {
        private bool _isEditing;
        private readonly Dictionary<string, object> _propertyTempValues 
                                    = new Dictionary<string, object>();

        public void Intercept(IInvocation invocation)
        {
            switch (invocation.Method.Name)
            {
                case "BeginEdit":
                    _isEditing = true;
                    return;
                case "CancelEdit":
                    _isEditing = false;
                    return;
                case "EndEdit":
                    _isEditing = false;
                    //Todo: this will raise the NotifyPropertyChanged!
                    //      if assign the values to the target... 
                    //      I don't know what to do when working without target.
                    AssignValues(invocation.Proxy);
                    return;
            }
            if (!_isEditing)
            {
                invocation.Proceed();
                return;
            }
            if(!invocation.Method.Name.StartsWith("set_") 
                && !invocation.Method.Name.StartsWith("get_"))
                return;

            var isSet = invocation.Method.Name.StartsWith("set_");
            var propertyName = invocation.Method.Name.Substring(4);

            if(isSet)
            {
                _propertyTempValues[propertyName] = invocation.Arguments[0];
            }
            else
            {
                if (_propertyTempValues.ContainsKey(propertyName))
                    invocation.ReturnValue = _propertyTempValues[propertyName];
                else
                    invocation.Proceed();
            }
        }

        private void AssignValues(object target)
        {
            foreach (var propertyTempValue in _propertyTempValues)
            {
                var property = target.GetType().GetProperty(propertyTempValue.Key);
                property.SetValue(target, propertyTempValue.Value, null);
            }
        }
    }
}