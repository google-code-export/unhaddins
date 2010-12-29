using System;
using uNhAddIns.Example.AspNetMVCConversationUsage.DatabaseAccessObjects;


namespace uNhAddIns.Example.AspNetMVCConversationUsage.Entities
{
    public class Product : IEntity {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int Price { get; set; }
        public virtual DateTime CreatedOn {get;set;}
        public virtual Category Category { get; set; }

    }
}
