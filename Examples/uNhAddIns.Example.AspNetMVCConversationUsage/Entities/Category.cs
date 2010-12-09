using System;
using System.Collections.Generic;
using uNhAddIns.Example.AspNetMVCConversationUsage.DatabaseAccessObjects;


namespace uNhAddIns.Example.AspNetMVCConversationUsage.Entities
{
    public class Category : IEntity, IWithSessionId {
		IList<Product> products = new List<Product>();

        public virtual IList<Product> Products {
            get { return products; }
            protected set { products = value;}
        }

        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime CreatedOn {get;set;}
        public virtual string NHibernateSessionId {get;set;}
		
		public virtual void AddProduct(Product product) {
			if (!products.Contains(product)) {
				products.Add(product);
				product.Category = this;
			}
		}
		public virtual void RemoveProduct(Product product) {
            if (products.Contains(product)) {
				products.Remove(product);
				product.Category = null;
			}
		}
    }
}