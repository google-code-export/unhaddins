using System;
using System.Collections.Generic;
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
        /// <typeparam name="TService"></typeparam>
        /// <param name="registration"></param>
        /// <returns></returns>
        public static ComponentRegistration<TService> NotifyOnPropertyChange<TService>(
            this ComponentRegistration<TService> registration)
        {
            ComponentRegistration<TService> result = registration;
            if (!typeof (INotifyPropertyChanged).IsAssignableFrom(typeof (TService)))
            {
                result = result.Proxy.AdditionalInterfaces(typeof (INotifyPropertyChanged));
            }

            result = result.Interceptors(new InterceptorReference(typeof (PropertyChangeInterceptor)))
                .Anywhere;

            return result;
        }

        public static ComponentRegistration<TService> EditableObjectBehavior<TService>(
            this ComponentRegistration<TService> componentRegistration)
        {
            ComponentRegistration<TService> result = componentRegistration;
            if (!typeof (IEditableObject).IsAssignableFrom(typeof (TService)))
            {
                result = result.Proxy.AdditionalInterfaces(typeof (IEditableObject));
            }
            result = result.Interceptors(new InterceptorReference(typeof (EditableObjectInterceptor)))
                .Anywhere;
            return result;
        }

        public static ComponentRegistration<TService> TargetIsCommonDatastore<TService>(
            this ComponentRegistration<TService> registration)
        {
            ComponentRegistration<TService> result = registration;
            if (!typeof (IProxiedEntity).IsAssignableFrom(typeof (TService)))
            {
                result = result.Proxy.AdditionalInterfaces(typeof (IProxiedEntity));
            }
            result = result.Interceptors(new InterceptorReference(typeof(EntityNameInterceptor))).Anywhere;
            return result.Interceptors(new InterceptorReference(typeof (CommonPropertyStoreInterceptor))).Last;
        }

        public static ComponentRegistration<TService> EnableNhibernateEntityCompatibility<TService>(
            this ComponentRegistration<TService> registration)
        {
            return registration.Proxy.AdditionalInterfaces(typeof (IProxiedEntity))
                .Interceptors(new InterceptorReference(typeof (EntityNameInterceptor))).Anywhere;
        }


        public static TEditableInterface GenerateEditableModel<TEditableInterface, TDomainInterface>(
            TDomainInterface entity)
            where TEditableInterface : INotifyPropertyChanged, IEditableObject, TDomainInterface
        {
            var proxyGen = new ProxyGenerator();

            return (TEditableInterface)
                   proxyGen.CreateInterfaceProxyWithTargetInterface(typeof (TDomainInterface),
                                                                    new[] {typeof (TEditableInterface)},
                                                                    entity,
                                                                    new ProxyGenerationOptions(),
                                                                    new IInterceptor[]
                                                                        {
                                                                            new EditableObjectInterceptor(),
                                                                            new PropertyChangeInterceptor()
                                                                        }
                       );
        }
    }
}