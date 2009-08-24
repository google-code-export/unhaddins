using System.ComponentModel;
using Castle.Core.Interceptor;
using uNhAddIns.WPF.Castle.BaseClasses;

namespace uNhAddIns.WPF.Castle
{
    public class PropertyChangeNotifier : PropertyChangeNotifierBase, IInterceptor
    {
        #region IInterceptor Members

        public void Intercept(IInvocation invocation)
        {
            string methodName = invocation.Method.Name;
            object[] arguments = invocation.Arguments;
            object proxy = invocation.Proxy;
            bool isEditableObject = proxy is IEditableObject;

            GrabEventsHandlers(methodName, arguments);

            if (!ShouldProceedWithInvocation(methodName))
                return;

            invocation.Proceed();

            NotifyPropertyChanged(methodName, proxy, isEditableObject);
        }

        #endregion
    }
}