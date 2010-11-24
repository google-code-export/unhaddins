<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<uNhAddIns.Example.AspNetMVCConversationUsage.Entities.Category>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	lazytest
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>lazytest</h2>
    
    <span class="header">These products was loaded lazily</span>
    <hr />
    <br />
    <div id="content">
    <table class="grid">
        <thead>
		    <th>Name</th>
            <th>Price</th>
		    <th>CreatedOn</th>
        </thead>
        <tbody>
     <% foreach (var item in Model.Products) { %>
         <tr>
          <td><%=item.Name %> </td>
	      <td><%=item.Price %> </td>
	      <td><%=item.CreatedOn.ToString("HH:mm") %> </td>
         </tr>
    <% } %>
        </tbody>
    </table>
    </div>

</asp:Content>
