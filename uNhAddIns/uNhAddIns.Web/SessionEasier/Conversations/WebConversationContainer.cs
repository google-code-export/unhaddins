using System.Collections.Generic;
using System.Web;
using uNhAddIns.SessionEasier.Conversations;

namespace uNhAddIns.Web.SessionEasier.Conversations {
    public class WebConversationContainer : AbstractConversationContainer {
        private bool? autoUnBind;
        public const string ConversationCurrentIdKey = "Current.ConversationId";
        public const string ConversationStoreKey = "NHibernate.Shared.Conersations";


        #region Overrides of AbstractConversationContainer

        protected override string CurrentId {
            get {
                return HttpContext.Current.Session[ConversationCurrentIdKey] as string;
            }
            set {
                HttpContext.Current.Session[ConversationCurrentIdKey] = value;
            }
        }

        protected override IDictionary<string, IConversation> Store {
            get {
                var store = HttpContext.Current.Session[ConversationStoreKey] as IDictionary<string, IConversation>;
                if (store == null) {
                    store = new Dictionary<string, IConversation>(10);
                    HttpContext.Current.Session[ConversationStoreKey] = store;
                }
                return store;
            }
        }
        #endregion

        public override bool AutoUnbindAfterEndConversation {
            // default autoUnbind is: true
            get { return autoUnBind.HasValue ? autoUnBind.Value : true; }
            set { autoUnBind = value; }
        }
    }
}