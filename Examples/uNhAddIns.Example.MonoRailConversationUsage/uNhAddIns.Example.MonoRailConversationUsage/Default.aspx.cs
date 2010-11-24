using System;

namespace uNhAddIns.Example.MonoRailConversationUsage {
    public partial class _Default : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            Response.Redirect("~/test/index.rails");
            base.OnLoad(e);
        }
    }
}
