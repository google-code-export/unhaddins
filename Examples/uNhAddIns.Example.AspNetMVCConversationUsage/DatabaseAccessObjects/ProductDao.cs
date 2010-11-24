using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using uNhAddIns.Example.AspNetMVCConversationUsage.Entities;



namespace uNhAddIns.Example.AspNetMVCConversationUsage.DatabaseAccessObjects
{
    public class ProductDao : BaseDao<Product>, IProductDao {
        public ProductDao(ISessionFactory factory) : base(factory) { }

        public IList<Product> FindByName(string name) {
            var criteria = factory.GetCurrentSession()
                .CreateCriteria(typeof(Product))
                .Add(Restrictions.Like("Name", name, MatchMode.Start)); // WARN: Magic string
            var list = criteria.List<Product>();
            return list;
        }
    }
}