using System.Collections.Generic;
using uNhAddIns.Example.MonoRailConversationUsage.Entities;

namespace uNhAddIns.Example.MonoRailConversationUsage.DatabaseAccessObjects {
    public interface IProductDao : IDao<Product> {
        IList<Product> FindByName(string name);
    }
}