using System;

namespace uNHAddIns.Examples.CustomInterceptor.Infrastructure
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class NotifyOnChangeAttribute : Attribute
    {
    }
}