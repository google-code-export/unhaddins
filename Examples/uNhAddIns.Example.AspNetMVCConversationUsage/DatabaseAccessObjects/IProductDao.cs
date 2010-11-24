using System.Collections.Generic;
using uNhAddIns.Example.AspNetMVCConversationUsage.Entities;


namespace uNhAddIns.Example.AspNetMVCConversationUsage.DatabaseAccessObjects
{
    public interface IProductDao : IDao<Product> {
        IList<Product> FindByName(string name);
    }
}