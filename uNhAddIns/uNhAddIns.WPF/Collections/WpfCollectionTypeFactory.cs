using NHibernate.Type;
using uNhAddIns.WPF.Collections.Types;

namespace uNhAddIns.WPF.Collections
{
    public class WpfCollectionTypeFactory : DefaultCollectionTypeFactory
    {
        public override CollectionType Bag<T>(string role, string propertyRef, bool embedded)
        {
            return new ObservableBagType<T>(role, propertyRef, embedded);
        }
        public override CollectionType Set<T>(string role, string propertyRef, bool embedded)
        {
            return new ObservableSetType<T>(role, propertyRef, embedded);
        }
        public override CollectionType List<T>(string role, string propertyRef, bool embedded)
        {
            return new ObservableListType<T>(role, propertyRef, embedded);
        }
    }
}