using System;
using System.Collections.Generic;
using Castle.Core;
using Castle.Core.Interceptor;

namespace uNHAddIns.Examples.CustomInterceptor.Infrastructure.MethodsInterceptors
{
    /// <summary>
    /// This interceptor act as a target and store the properties in a dictionary.
    /// Extracted from the article "Less than few is gof" from Fabio Maulo's blog.
    /// </summary>
    public class CommonPropertyStoreInterceptor : IInterceptor
    {
        private readonly Dictionary<string, object> _data = new Dictionary<string, object>(50);

        public void Intercept(IInvocation invocation)
        {
            string methodName = invocation.Method.Name;

            if (methodName.StartsWith("set_"))
            {
                string propertyName = methodName.Substring(4);
                _data[propertyName] = invocation.Arguments[0];
            }
            else if (methodName.StartsWith("get_"))
            {
                string propertyName = methodName.Substring(4);
                object value;
                _data.TryGetValue(propertyName, out value);
                if(value == null)
                    if (invocation.Method.ReturnType.IsValueType)
                    {
                        value = Activator.CreateInstance(invocation.Method.ReturnType);
                    }else if (invocation.Method.ReturnType.Equals(typeof(String)))
                    {
                        value = string.Empty;
                    }
                invocation.ReturnValue = value;
            }
            else if ("ToString".Equals(methodName))
            {
                IProxiedEntity proxiedEntity = invocation.Proxy as IProxiedEntity;
                if(proxiedEntity != null)
                {
                    invocation.ReturnValue = proxiedEntity.EntityName + "#" + _data["Id"];    
                }
                
            }
            else if ("GetHashCode".Equals(methodName))
            {
                invocation.ReturnValue = GetHashCode();
            }
            else
            {
                invocation.Proceed();
            }
        }


    }
}