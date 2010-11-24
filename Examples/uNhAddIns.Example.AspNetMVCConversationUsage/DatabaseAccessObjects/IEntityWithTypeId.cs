namespace uNhAddIns.Example.AspNetMVCConversationUsage.DatabaseAccessObjects
{
    public interface IEntityWithTypeId<TIdType> {
        TIdType Id { get; set; }
    }
}