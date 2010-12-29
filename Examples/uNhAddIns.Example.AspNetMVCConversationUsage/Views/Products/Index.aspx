<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<uNhAddIns.Example.AspNetMVCConversationUsage.Entities.Category>>" %>
<%@ Import Namespace="uNhAddIns.Example.AspNetMVCConversationUsage.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <span>Listing committed categories of product:</span>
    <br />
    <div id="content">
        <table class="grid">
        <thead>
		<th>Name</th>
		<th>SessionId</th>
		<th>CreatedOn</th>
		<th>Delete and Commit</th>
		<th>Products (Conversation)</th>
		<th>Products (Session Per Request, expect exception)</th>
    </thead>
    <tbody>
   <% foreach (var item in Model) { %>
         <tr>
          <td><%=item.Name %> </td>
	      <td><%=item.NHibernateSessionId %></td>
	      <td><%=item.CreatedOn.ToString("HH:mm")%></td>
	      <td><%=Html.ActionLink<ProductsController>(x => x.Delete(item.Id), "Delete")%> </td>
	      <td><%=Html.ActionLink<ProductsController>(x => x.GetProducts(item.Id), "Get")%> </td>
	      <td><%=Html.ActionLink<ProductsController>(x => x.GetProductsWithLazyLoadException(item.Id), "Get")%> </td>
         </tr>
    <% } %>
    </tbody>
</table>
</div>
<hr />
The last action was:
<span class="redtext"><%= ViewData["message"]%></span>
<br />
   
</asp:Content>
