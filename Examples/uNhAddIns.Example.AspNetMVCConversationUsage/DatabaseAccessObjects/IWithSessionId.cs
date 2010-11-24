namespace uNhAddIns.Example.AspNetMVCConversationUsage.DatabaseAccessObjects
{
    public interface IWithSessionId {
        string NHibernateSessionId { get; set; }
    }
}