using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Iesi.Collections;
using Iesi.Collections.Generic;
using NHibernate.Collection.Generic;
using NHibernate.Engine;
using NHibernate.Persister.Collection;

namespace uNhAddIns.WPF.Collections.PersistentImpl
{
    public class PersistentObservableGenericSet<T> : PersistentGenericSet<T>, INotifyCollectionChanged,
                                                     INotifyPropertyChanged, IEditableObject
    {
        private NotifyCollectionChangedEventHandler _collectionChanged;
        private PropertyChangedEventHandler _propertyChanged;

        public PersistentObservableGenericSet(ISessionImplementor sessionImplementor)
            : base(sessionImplementor)
        {
        }

        public PersistentObservableGenericSet(ISessionImplementor sessionImplementor, ISet<T> coll)
            : base(sessionImplementor, coll)
        {
            CaptureEventHandlers(coll);
        }

        public PersistentObservableGenericSet()
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
            CaptureEventHandlers(set);
        }

        private void CaptureEventHandlers(object coll)
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

        /// <summary>
        /// Begins an edit on an object.
        /// </summary>
        public void BeginEdit()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Pushes changes since the last <see cref="M:System.ComponentModel.IEditableObject.BeginEdit"/> or <see cref="M:System.ComponentModel.IBindingList.AddNew"/> call into the underlying object.
        /// </summary>
        public void EndEdit()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Discards changes since the last <see cref="M:System.ComponentModel.IEditableObject.BeginEdit"/> call.
        /// </summary>
        public void CancelEdit()
        {
            throw new NotImplementedException();
        }
    }
}