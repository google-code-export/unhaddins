using System;

namespace uNhAddIns.ComponentBehaviors.Castle.Configuration
{
    public interface IBehaviorConfigurator
    {
        ProxyInformation GetProxyInformation(Type implementationType);
    }
}