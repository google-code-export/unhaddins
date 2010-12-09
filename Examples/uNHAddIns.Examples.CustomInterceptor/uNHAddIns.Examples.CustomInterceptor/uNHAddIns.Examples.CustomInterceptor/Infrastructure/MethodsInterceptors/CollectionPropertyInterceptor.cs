using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using Castle.Core.Interceptor;
using Castle.DynamicProxy;

namespace uNHAddIns.Examples.CustomInterceptor.Infrastructure.MethodsInterceptors
{

    

    public class CollectionPropertyInterceptor : IInterceptor
    {

        private readonly IDictionary<string, object> _interceptedCollections
            = new Dictionary<string, object>();

        private object[] AttributeLookup(Type type, Type attribType, string propertyName) 
        {
            var property = type.GetProperty(propertyName);
            if (property == null)
            {
                return new object[]{};
            }
            
            var attributes = property.GetCustomAttributes(attribType,true);

            if(attributes.Length > 0)
            {
                return attributes;
            }
            if (type.BaseType == null)
            {
                return new object[]{};
            }
            return AttributeLookup(type.BaseType, attribType, propertyName);
        }

        #region IInterceptor Members

        public void Intercept(IInvocation invocation)
        {
            //invocation.Proceed();
            if (!invocation.MethodInvocationTarget.Name.StartsWith("get_"))
            {
                invocation.Proceed();
                return;
            }

            string propName = invocation.MethodInvocationTarget.Name.Substring(4);

            //var property = invocation.InvocationTarget.GetType().GetProperty(propName);

            if (AttributeLookup(invocation.InvocationTarget.GetType(),
                                typeof (NotifyOnChangeAttribute),
                                propName).Length != 1)
            {
                invocation.Proceed();
                return;
            }


            object interceptedCollection;
            _interceptedCollections.TryGetValue(propName, out interceptedCollection);

            if (interceptedCollection != null)
            {
                invocation.ReturnValue = interceptedCollection;
            }
            else
            {
                invocation.Proceed();

                //var proxyGenerator = new ProxyGenerator();
                var itemType = invocation.ReturnValue.GetType()
                                         .GetGenericArguments()[0];

                var observableType = typeof (ObservableCollection<>)
                                            .MakeGenericType(itemType);

                object newInterceptedCollection = 
                    Activator.CreateInstance(observableType, invocation.ReturnValue);

                //new ObservableCollection<string>()

                    
                    //proxyGenerator.CreateInterfaceProxyWithTargetInterface(
                    //property.PropertyType, new[]
                    //                           {
                    //                               typeof (INotifyCollectionChanged), 
                    //                               typeof(INotifyPropertyChanged),
                    //                               typeof(IList)
                    //                           },
                    //invocation.ReturnValue,
                    //ProxyGenerationOptions.Default,
                    //new[] {new ListMethodCallInterceptor()});

                invocation.ReturnValue = newInterceptedCollection;

                _interceptedCollections.Add(propName, newInterceptedCollection);
            }
        }

        #endregion
    }
}