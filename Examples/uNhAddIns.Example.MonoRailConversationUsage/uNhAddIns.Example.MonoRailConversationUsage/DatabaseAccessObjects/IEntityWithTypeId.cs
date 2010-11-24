namespace uNhAddIns.Example.MonoRailConversationUsage.DatabaseAccessObjects {
    public interface IEntityWithTypeId<TIdType> {
        TIdType Id { get; set; }
    }
}