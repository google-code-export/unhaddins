using System;
using System.Collections.Generic;
using Castle.Core;
using Castle.Core.Interceptor;

namespace uNHAddIns.Examples.CustomInterceptor.Infrastructure.MethodsInterceptors
{
    public class DataInterceptor : IInterceptor
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