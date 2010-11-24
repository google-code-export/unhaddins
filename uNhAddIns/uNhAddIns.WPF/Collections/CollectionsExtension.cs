using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using NHibernate;

namespace uNhAddIns.WPF.Collections
{
    public static class CollectionsExtension
    {
        public static ObservableCollection<T> AsObservable<T>(this IEnumerable<T> coll)
        {
            return new ObservableCollection<T>(coll);
        }

        /// <summary>
        /// untested.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public static ObservableCollection<T> AsObservableWithAutoPersistence<T>(this IEnumerable<T> coll,
                                                                                 ISession session)
        {
            var observableCollection = new ObservableCollection<T>(coll);

            observableCollection.CollectionChanged
                += (sender, args) =>
                       {
                           switch (args.Action)
                           {
                               case NotifyCollectionChangedAction.Add:
                                   foreach (object newItem in args.NewItems)
                                       session.Save(newItem);
                                   break;
                               case NotifyCollectionChangedAction.Remove:
                                   foreach (object newItem in args.OldItems)
                                       session.Delete(newItem);
                                   break;
                               case NotifyCollectionChangedAction.Reset:
                                   //ouch
                                   break;
                               default:
                                   break;
                           }
                       };
            return observableCollection;
        }


        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }            
        }

        public static void ForEach(this IEnumerable enumerable, Action<object> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }            
        }
    }
}