using System;
using System.ComponentModel;
using System.Linq;
using Castle.Core;
using Castle.Core.Interceptor;
using Castle.DynamicProxy;
using Castle.MicroKernel;
using Castle.MicroKernel.Proxy;
using Castle.MicroKernel.Registration;
using uNHAddIns.Examples.CustomInterceptor.Domain;
using uNHAddIns.Examples.CustomInterceptor.Infrastructure.MethodsInterceptors;

namespace uNHAddIns.Examples.CustomInterceptor.Infrastructure
{
    public static class WindsorRegistrationUtil
    {
       
        /// <summary>
        /// Add the PropertyChangeInterceptor to the component.
        /// </summary>
        /// <typeparam name="TConcreteType"></typeparam>
        /// <param name="registration"></param>
        /// <returns></returns>
        public static ComponentRegistration<TConcreteType> WithNotificablePropertyChanged<TConcreteType>(this ComponentRegistration<TConcreteType> registration)
        {
            var result = registration;
            if(!typeof(INotifyPropertyChanged).IsAssignableFrom(typeof(TConcreteType)))
            {
                throw new Exception(string.Format("{0} must implement INotifyPropertyChanged.", 
                                                  typeof(TConcreteType).FullName.ToString()));
            }
            if(!typeof(IProxiedEntity).IsAssignableFrom(typeof(TConcreteType)))
            {
                result = result.Proxy.AdditionalInterfaces(typeof (IProxiedEntity));
            }
            result = result.Interceptors(new InterceptorReference(typeof(PropertyChangeInterceptor))).Anywhere;
            return result;
        }

       
        public static ComponentRegistration<TType> TargetIsCommonDatastore<TType>(this ComponentRegistration<TType> registration)
        {
            return registration.UsingFactoryMethod(kernel => CreateProxy<TType>(kernel));
        }

        /// <summary>
        /// Factory method to create a proxie.
        /// This method take cares about the preexistent interceptors and additional interfaces.
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="kernel"></param>
        /// <returns></returns>
        private static TType CreateProxy<TType>(IKernel kernel)
        {
            var componentModel = kernel.GetHandler(typeof(TType)).ComponentModel;
            var currentInterceptorsReferences = componentModel.Interceptors;
            
            var currentInterceptors = currentInterceptorsReferences
                .Select(refe => (IInterceptor)kernel.Resolve(refe.ServiceType))
                .ToArray();

            var newInterceptors = new IInterceptor[]
                                      {
                                          new MethodsInterceptors.EntityNameInterceptor(typeof (IProduct).FullName),
                                          new DataInterceptor()
                                      };

            var interceptors = currentInterceptors.Union(newInterceptors).ToArray();

            var additionalInterfaces = ProxyUtil.ObtainProxyOptions(componentModel, true)
                .AdditionalInterfaces.ToList();
            
            if(!additionalInterfaces.Contains(typeof(IProxiedEntity)))
                additionalInterfaces.Add(typeof(IProxiedEntity));

            var interfaces = additionalInterfaces.ToArray();

            
            var proxyGen = new Castle.DynamicProxy.ProxyGenerator();
            return (TType) proxyGen.CreateInterfaceProxyWithoutTarget(
                               typeof(TType),
                               interfaces,
                               interceptors);
        }
    }
}