using System;
using System.ComponentModel;
using Castle.Core;
using Castle.Core.Interceptor;

namespace uNHAddIns.Examples.CustomInterceptor.Infrastructure.MethodsInterceptors
{
    public class PropertyChangeInterceptor : IInterceptor, IOnBehalfAware
    {
        private string _entityName = string.Empty;
        private PropertyChangedEventHandler _handler;

        #region IInterceptor Members

        public void Intercept(IInvocation invocation)
        {
            string methodName = invocation.Method.Name;
            if (methodName == "add_PropertyChanged")
            {
                _handler = (PropertyChangedEventHandler) 
                    Delegate.Combine(_handler, (Delegate) invocation.Arguments[0]);
                return;
            }
            if (methodName == "remove_PropertyChanged")
            {
                _handler = (PropertyChangedEventHandler) 
                    Delegate.Remove(_handler, (Delegate) invocation.Arguments[0]);
                return;
            }
            //if (methodName == "get_EntityName" && !string.IsNullOrEmpty(_entityName))
            //{
            //    invocation.ReturnValue = _entityName;
            //    return;
            //}

            invocation.Proceed();

            if (invocation.MethodInvocationTarget.Name.StartsWith("set_"))
            {
                if (typeof (INotifyPropertyChanged).IsAssignableFrom(invocation.Proxy.GetType()))
                {
                    if (_handler != null)
                        _handler(invocation.Proxy, new PropertyChangedEventArgs(methodName.Substring(4)));
                }
            }
        }

        #endregion

        #region IOnBehalfAware Members

        public void SetInterceptedComponentModel(ComponentModel target)
        {
            if (target != null)
            {
                _entityName = target.Implementation.FullName;
            }
        }

        #endregion
    }
}