using System;
using System.ComponentModel;

namespace uNHAddIns.Examples.CustomInterceptor.Domain
{
    public class Customer : INotifyPropertyChanged, IEditableObject
    {
        //don't raise this event... Is aop baby.
        public virtual event PropertyChangedEventHandler PropertyChanged;

        //public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Address { get; set; }
        public virtual void BeginEdit()
        {}

        public virtual void EndEdit()
        {}

        public virtual void CancelEdit()
        {}
    }
}