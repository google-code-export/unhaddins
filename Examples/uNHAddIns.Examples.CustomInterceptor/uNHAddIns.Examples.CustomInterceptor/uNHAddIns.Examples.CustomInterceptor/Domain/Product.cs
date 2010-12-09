using System;
using System.ComponentModel;
using Castle.Core;

namespace uNHAddIns.Examples.CustomInterceptor.Domain
{
    /// <summary>
    /// Look there is not *any* real implementation.
    /// </summary>
    public interface IProduct : INotifyPropertyChanged, IEditableObject
    {
        Guid Id { get; set; }
        string Description { get; set; }
        decimal UnitPrice { get; set; }

        [DoNotWire]
        IStore Store { get; set; }
    }
}