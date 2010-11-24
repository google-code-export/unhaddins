namespace uNhAddIns.Example.MonoRailConversationUsage.DatabaseAccessObjects {
    public interface IWithSessionId {
        string NHibernateSessionId { get; set; }
    }
}