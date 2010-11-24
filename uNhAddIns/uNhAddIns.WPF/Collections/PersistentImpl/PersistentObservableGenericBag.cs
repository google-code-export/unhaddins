using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using NHibernate.Collection.Generic;
using NHibernate.Engine;
using NHibernate.Persister.Collection;

namespace uNhAddIns.WPF.Collections.PersistentImpl
{
    public class PersistentObservableGenericBag<T> : PersistentGenericBag<T>, INotifyCollectionChanged,
                                                     INotifyPropertyChanged, IList<T>
    {
        private NotifyCollectionChangedEventHandler _collectionChanged;
        private PropertyChangedEventHandler _propertyChanged;

        public PersistentObservableGenericBag(ISessionImplementor sessionImplementor)
            : base(sessionImplementor)
        {
        }

        public PersistentObservableGenericBag(ISessionImplementor sessionImplementor, ICollection<T> coll)
            : base(sessionImplementor, coll)
        {
            CaptureEventHandlers(coll);
        }

        public PersistentObservableGenericBag()
        {
        }

        #region INotifyCollectionChanged Members

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                Initialize(false);
                _collectionChanged += value;
            }
            remove { _collectionChanged -= value; }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                Initialize(false);
                _propertyChanged += value;
            }
            remove { _propertyChanged += value; }
        }

        #endregion

        public override void BeforeInitialize(ICollectionPersister persister, int anticipatedSize)
        {
            base.BeforeInitialize(persister, anticipatedSize);
            CaptureEventHandlers(InternalBag);
        }

        private void CaptureEventHandlers(ICollection<T> coll)
        {
            var notificableCollection = coll as INotifyCollectionChanged;
            var propertyNotificableColl = coll as INotifyPropertyChanged;

            if (notificableCollection != null)
                notificableCollection.CollectionChanged += OnCollectionChanged;

            if (propertyNotificableColl != null)
                propertyNotificableColl.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler changed = _propertyChanged;
            if (changed != null) changed(this, e);
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler changed = _collectionChanged;
            if (changed != null) changed(this, e);
        }
    }
}