using System;
using uNhAddIns.Example.MonoRailConversationUsage.DatabaseAccessObjects;

namespace uNhAddIns.Example.MonoRailConversationUsage.Entities {
    public class Product : IEntity {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int Price { get; set; }
        public virtual DateTime CreatedOn {get;set;}
        public virtual Category Category { get; set; }

    }
}
