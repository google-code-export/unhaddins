using System;
using NHibernate;
using NHibernate.Context;
using NHibernate.Engine;
using uNhAddIns.SessionEasier.Conversations;

namespace uNhAddIns.Web.SessionEasier.Conversations {
    [Serializable]
    public class ConversationWebSessionContext : WebConversationContainer, ICurrentSessionContext {

        private readonly ISessionFactoryImplementor factory;

        public ConversationWebSessionContext(ISessionFactoryImplementor factory)
        {
            this.factory = factory;
        }


        #region Implementation of ICurrentSessionContext

        public ISessionFactoryImplementor Factory
        {
            get { return factory; }
        }

        public ISession CurrentSession()
        {
            var c = CurrentConversation as NhConversation;
            if(c == null)
            {
                throw new ConversationException("No current conversation available. Create a conversation and bind it to the container.");
            }
            return c.GetSession(Factory);
        }
        #endregion
    }
}