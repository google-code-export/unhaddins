using System;

namespace uNhAddIns.NHibernateTypeResolver
{
    //This is the marker.

    public interface IWellKnownProxy
    {
        string EntityName { get; }
        Type EntityType { get; }
    }
}