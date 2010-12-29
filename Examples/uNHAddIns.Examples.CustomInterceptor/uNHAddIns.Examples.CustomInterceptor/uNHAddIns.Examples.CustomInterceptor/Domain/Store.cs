using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using NHibernate.Properties;
using uNHAddIns.Examples.CustomInterceptor.Infrastructure;

namespace uNHAddIns.Examples.CustomInterceptor.Domain
{
    
    public interface IStore
    {
        string Name { get; set; }

        IList<IProduct> Products { get; }
        void AddProduct(IProduct product);
        void RemoveProduct(IProduct product);
        void RemoveProductAt(int index);
        void RemoveAllProducts();
        void InsertProduct(int index, IProduct product);
    }

    public class Store : IStore
    {
        public virtual string Name { get; set; }

        [NotifyOnChange]
        public virtual IList<IProduct> Products { get; private set; }

        public Store()
        {
            Products = new List<IProduct>();
        }

        public virtual void AddProduct(IProduct product)
        {
            product.Store = this;
            Products.Add(product);
        }

        public virtual void RemoveProduct(IProduct product)
        {
            product.Store = null;
            Products.Remove(product);
        }

        public virtual void RemoveProductAt(int index)
        {
            Products[index].Store = null;
            Products.RemoveAt(index);
        }

        public virtual void RemoveAllProducts()
        {
            foreach (var prod in Products)
                prod.Store = null;
            Products.Clear();
        }

        public virtual void InsertProduct(int index, IProduct product)
        {
            product.Store = this;
            Products.Insert(index, product);
        }
    }

    /// <summary>
    /// This interface is used from a WPF gui.
    /// </summary>
    public interface IEditableCategoryModel : IStore, INotifyPropertyChanged, IEditableObject
    { }
}